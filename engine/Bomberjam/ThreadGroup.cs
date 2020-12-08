using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Bomberjam
{
    internal sealed class ThreadGroup : IDisposable
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly CancellationToken _token;
        private readonly List<Thread> _threads;
        private readonly Stopwatch _watch;
        private bool _isCompleted;

        public ThreadGroup()
        {
            this._tokenSource = new CancellationTokenSource();
            this._token = this._tokenSource.Token;
            this._threads = new List<Thread>();
            this._watch = new Stopwatch();
        }

        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;

        public void ExecuteThread<T>(Action<T, CancellationToken> action, T state)
        {
            if (this._isCompleted || this._tokenSource.IsCancellationRequested)
            {
                throw new InvalidOperationException("Cannot add more threads");
            }

            if (!this._watch.IsRunning)
            {
                this._watch.Start();
            }

            var thread = new Thread(() => action(state, this._token))
            {
                IsBackground = true
            };

            thread.Start();

            this._threads.Add(thread);
        }

        public void WaitForCompletion(TimeSpan timeout)
        {
            if (this._tokenSource.IsCancellationRequested)
            {
                throw new InvalidOperationException("Thread canceling has been requested");
            }

            if (timeout != TimeSpan.MaxValue)
            {
                this._tokenSource.CancelAfter(timeout);
            }

            foreach (var thread in this._threads)
            {
                try
                {
                    if (timeout == TimeSpan.MaxValue)
                    {
                        thread.Join();
                    }
                    else
                    {
                        thread.Join((int)timeout.TotalMilliseconds);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    // ignored
                }
                catch (OperationCanceledException)
                {
                    // ignored
                }
            }

            this._isCompleted = true;
            this._watch.Stop();

            this.Elapsed = this._watch.Elapsed;
        }

        public void Dispose()
        {
            this._tokenSource.Dispose();

            if (!this._isCompleted)
            {
                foreach (var thread in this._threads)
                {
                    try
                    {
                        if (thread.IsAlive)
                        {
                            thread.Abort();
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                this._threads.Clear();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Bomberjam.Website.Storage
{
    public sealed class CompositeBomberjamStorage : IBomberjamStorage
    {
        private readonly IReadOnlyList<IBomberjamStorage> _storages;

        public CompositeBomberjamStorage(IReadOnlyList<IBomberjamStorage> storages)
        {
            Debug.Assert(storages != null);
            Debug.Assert(storages.Count > 0);
            this._storages = storages;
        }

        private async Task Wrap(Func<IBomberjamStorage, Task> action)
        {
            for (var i = 0; i < this._storages.Count; i++)
            {
                var index = i;

                try
                {
                    await action(this._storages[index]).ConfigureAwait(false);
                    return;
                }
                catch
                {
                    if (index == this._storages.Count - 1)
                    {
                        throw;
                    }
                }
            }

            throw new InvalidOperationException();
        }

        public async Task UploadBotSourceCode(Guid botId, Stream fileStream)
        {
            await this.Wrap(storage => storage.UploadBotSourceCode(botId, fileStream)).ConfigureAwait(false);
        }

        public async Task UploadCompiledBot(Guid botId, Stream fileStream)
        {
            await this.Wrap(storage => storage.UploadCompiledBot(botId, fileStream)).ConfigureAwait(false);
        }

        public async Task UploadGameResult(Guid botId, Stream fileStream)
        {
            await this.Wrap(storage => storage.UploadGameResult(botId, fileStream)).ConfigureAwait(false);
        }

        public async Task DownloadBotSourceCodeTo(Guid botId, Stream destinationStream)
        {
            await this.Wrap(storage => storage.DownloadBotSourceCodeTo(botId, destinationStream)).ConfigureAwait(false);
        }

        public async Task DownloadCompiledBotTo(Guid botId, Stream destinationStream)
        {
            await this.Wrap(storage => storage.DownloadCompiledBotTo(botId, destinationStream)).ConfigureAwait(false);
        }

        public async Task DownloadGameResultTo(Guid botId, Stream destinationStream)
        {
            await this.Wrap(storage => storage.DownloadGameResultTo(botId, destinationStream)).ConfigureAwait(false);
        }
    }
}
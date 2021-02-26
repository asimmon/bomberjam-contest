using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Bomberjam.Website.Utils
{
    public sealed class PushFileStreamResult : FileResult
    {
        public PushFileStreamResult(string contentType, string fileDownloadName, Func<Stream, Task> streamingCallback)
            : base(MediaTypeHeaderValue.Parse(contentType).ToString())
        {
            this.StreamingCallback = streamingCallback ?? throw new ArgumentNullException(nameof(streamingCallback));
            this.FileDownloadName = fileDownloadName;
        }

        public Func<Stream, Task> StreamingCallback { get; }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var executor = context.HttpContext.RequestServices.GetRequiredService<PushFileStreamResultExecutor>();
            return executor.ExecuteAsync(context, this);
        }
    }
}
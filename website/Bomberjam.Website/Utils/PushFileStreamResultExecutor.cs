using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Utils
{
    public sealed class PushFileStreamResultExecutor : FileResultExecutorBase, IActionResultExecutor<PushFileStreamResult>
    {
        public PushFileStreamResultExecutor(ILoggerFactory loggerFactory)
            : base(CreateLogger<PushFileStreamResultExecutor>(loggerFactory))
        {
        }

        public Task ExecuteAsync(ActionContext context, PushFileStreamResult result)
        {
            // File length cannot be determined from the streaming callback
            this.SetHeadersAndLog(context, result, fileLength: null, result.EnableRangeProcessing, result.LastModified, result.EntityTag);
            return result.StreamingCallback(context.HttpContext.Response.Body);
        }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bomberjam.Website.Utils
{
    public static class ControllerBaseExtensions
    {
        public static PushFileStreamResult PushFileStream(this ControllerBase _, string contentType, string fileDownloadName, Func<Stream, Task> streamingCallback)
        {
            return new PushFileStreamResult(contentType, fileDownloadName, streamingCallback);
        }
    }
}
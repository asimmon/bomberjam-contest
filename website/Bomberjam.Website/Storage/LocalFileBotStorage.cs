using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Bomberjam.Website.Storage
{
    public sealed class LocalFileBotStorage : IBotStorage
    {
        private readonly string _basePath;

        public LocalFileBotStorage(string basePath)
        {
            this._basePath = Path.Combine(basePath, "Bomberjam");
            Directory.CreateDirectory(this._basePath);
        }

        public int GetFileCount()
        {
            return Directory.GetFiles(this._basePath).Length;
        }

        public Task UploadBotSourceCode(int userId, Stream fileStream)
        {
            return this.UploadBotSourceCode(userId, false, fileStream);
        }

        public Task UploadCompiledBot(int userId, Stream fileStream)
        {
            return this.UploadBotSourceCode(userId, true, fileStream);
        }

        private async Task UploadBotSourceCode(int userId, bool isCompiled, Stream fileStream)
        {
            var filePath = Path.Join(this._basePath, MakeFileName(userId, isCompiled));
            await using (fileStream)
            {
                await using var localFile = File.Open(filePath, FileMode.Create);
                await fileStream.CopyToAsync(localFile);
            }
        }

        private static string MakeFileName(int userId, bool isCompiled)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:00000000}-{1}.zip", userId, isCompiled ? 1 : 0);
        }

        public Stream DownloadBotSourceCode(int userId)
        {
            return this.DownloadBotSourceCode(userId, false);
        }

        public Stream DownloadCompiledBot(int userId)
        {
            return this.DownloadBotSourceCode(userId, true);
        }

        private Stream DownloadBotSourceCode(int userId, bool isCompiled)
        {
            var filePath = Path.Join(this._basePath, MakeFileName(userId, isCompiled));
            return File.OpenRead(filePath);
        }
    }
}
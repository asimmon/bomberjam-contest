using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bomberjam.Website.Setup
{
    public class AddDebugUsersOnStartup : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AddDebugUsersOnStartup(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = this._serviceProvider.CreateScope())
            {
                var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
                if (!environment.IsDevelopment())
                    return;

                var repository = scope.ServiceProvider.GetRequiredService<IBomberjamRepository>();
                var storage = scope.ServiceProvider.GetRequiredService<IBomberjamStorage>();

                await AddUser("36072624", "Minty", repository, storage);
                await AddUser("9208753", "Kalmera", repository, storage);
                await AddUser("26142591", "Pandarf", repository, storage);
                await AddUser("5122918", "Falgar", repository, storage);

                await repository.UpdateAllUserGlobalRanks();
            }
        }

        private static async Task AddUser(string githubId, string username, IBomberjamRepository repository, IBomberjamStorage storage)
        {
            var zippedBotFileStream = typeof(Startup).Assembly.GetManifestResourceStream("Bomberjam.Website.RandomBot.zip");
            if (zippedBotFileStream == null)
                return;

            try
            {
                await repository.GetUserByGithubId(githubId);
                return;
            }
            catch (EntityNotFound)
            {
            }

            using (var transaction = await repository.CreateTransaction())
            {
                var user = await repository.AddUser(githubId, username);
                var newBotId = await repository.AddBot(user.Id);

                await using (zippedBotFileStream)
                {
                    await using var zippedBotFileMs = new MemoryStream();
                    await zippedBotFileStream.CopyToAsync(zippedBotFileMs);
                    zippedBotFileMs.Seek(0, SeekOrigin.Begin);
                    await storage.UploadBotSourceCode(newBotId, zippedBotFileMs);
                }

                await repository.AddCompilationTask(newBotId);
                await transaction.CommitAsync();
            }
        }
    }
}
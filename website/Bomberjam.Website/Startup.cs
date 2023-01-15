using System;
using System.Net.Http.Headers;
using Bomberjam.Website.Authentication;
using Bomberjam.Website.Configuration;
using Bomberjam.Website.Database;
using Bomberjam.Website.Github;
using Bomberjam.Website.Jobs;
using Bomberjam.Website.Logging;
using Bomberjam.Website.Setup;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this._configuration = configuration;
            this._environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // options & configuration
            services.AddOptions<SecretAuthenticationOptions>().BindConfiguration("SecretAuthentication").ValidateDataAnnotations().ValidateOnStart();
            services.AddOptions<ConnectionStringOptions>().BindConfiguration("ConnectionStrings").ValidateDataAnnotations().ValidateOnStart();
            services.AddOptions<TelemetryOptions>().BindConfiguration("Telemetry").ValidateDataAnnotations().ValidateOnStart();
            services.AddOptions<GitHubOptions>().BindConfiguration("GitHub").ValidateDataAnnotations().ValidateOnStart();
            services.AddOptions<JobOptions>().BindConfiguration("Jobs").ValidateDataAnnotations().ValidateOnStart();

            services.ConfigureOptions<MvcSetup>();
            services.ConfigureOptions<AuthenticationSetup>();

            // mvc
            var mvcBuilder = services.AddMvc();
            if (this._environment.IsDevelopment())
                mvcBuilder.AddRazorRuntimeCompilation();

            services.AddResponseCompression();
            services.AddAuthentication().AddCookie().AddGitHub().AddSecret();
            services.AddScoped<PushFileStreamResultExecutor>();

            // database
            services.AddDbContext<BomberjamContext>((serviceProvider, builder) =>
            {
                var scopedConnectionStrings = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>();
                var sqlBuilder = builder.UseSqlServer(scopedConnectionStrings.Value.BomberjamContext);
                if (this._environment.IsDevelopment())
                    sqlBuilder.EnableSensitiveDataLogging();
            });

            if (this._environment.IsDevelopment())
                services.AddDatabaseDeveloperPageExceptionFilter();

            // repo & storage
            services.AddSingleton<IBomberjamStorage>(serviceProvider =>
            {
                var scopedConnectionStrings = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>();
                var blobStorage = new AzureStorageBomberjamStorage(scopedConnectionStrings.Value.BomberjamStorage);
                return blobStorage;

                /*
                var scopedEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
                if (!scopedEnvironment.IsDevelopment())
                    return blobStorage;

                var tempStorage = new LocalFileBomberjamStorage(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                return new CompositeBomberjamStorage(tempStorage, blobStorage);
                //*/
            });

            services.AddSingleton<IObjectCache, ObjectCache>();
            services.AddScoped<DatabaseRepository>();
            services.AddScoped<IBomberjamRepository>(container =>
            {
                var repository = container.GetRequiredService<DatabaseRepository>();
                var objectCache = container.GetRequiredService<IObjectCache>();
                return new CachedDatabaseRepository(repository, objectCache);
            });

            // jobs
            services.AddHangfire((serviceProvider, builder) =>
            {
                var scopedConnectionStrings = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>();
                builder.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(scopedConnectionStrings.Value.BomberjamContext);
            });

            services.AddHangfireServer();

            // telemetry
            services.AddTransient<ITagHelperComponent, GoogleAnalyticsTagHelperComponent>();
            services.AddTransient<ITagHelperComponent, HotjarTagHelperComponent>();

            // github artifacts downloader
            services.AddHttpClient(nameof(GithubArtifactManager), (container, client) =>
            {
                var options = container.GetRequiredService<IOptions<GitHubOptions>>();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Bomberjam", productVersion: null));

                if (!string.IsNullOrEmpty(options.Value.ArtifactsUsername) && !string.IsNullOrEmpty(options.Value.ArtifactsPassword))
                {
                    client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(options.Value.ArtifactsUsername, options.Value.ArtifactsPassword);
                }
            });

            services.AddSingleton<IGithubArtifactManager, GithubArtifactManager>();
            services.AddHostedService<AddDebugUsersOnStartup>();
        }

        public void Configure(IApplicationBuilder app, IRecurringJobManager recurringJobs, IOptions<GitHubOptions> githubOptions, IOptions<JobOptions> jobOptions)
        {
            app.UseResponseCompression();

            if (this._environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Web/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseUserEnricher();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHangfireDashboard(new DashboardOptions
                {
                    Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
                });
            });

            recurringJobs.AddOrUpdate<MatchmakingJob>("matchmaking", job => job.Run(), jobOptions.Value.Matchmaking);
            recurringJobs.AddOrUpdate<OrphanedTaskFixingJob>("orphanedTasks", job => job.Run(), jobOptions.Value.OrphanedTasks);
            recurringJobs.AddOrUpdate<DownloadGithubArtifactsJob>("githubArtifacts", job => job.Run(), jobOptions.Value.GithubArtifacts);
            recurringJobs.Trigger("githubArtifacts");
        }
    }
}
using System.IO;
using Bomberjam.Website.Authentication;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Jobs;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddMvc();
            services.AddScoped<PushFileStreamResultExecutor>();
            services.AddSingleton(new GitHubConfiguration(this.Configuration));

            services.AddResponseCompression(opts =>
            {
                opts.EnableForHttps = true;
            });

            _ = this.Environment.IsDevelopment()
                ? services.AddControllersWithViews().AddRazorRuntimeCompilation()
                : services.AddControllersWithViews();

            services.Configure<FormOptions>(x => x.MultipartBodyLengthLimit = Constants.GeneralMaxUploadSize);
            services.Configure<KestrelServerOptions>(x => x.Limits.MaxRequestBodySize = Constants.GeneralMaxUploadSize);

            this.ConfigureAuthentication(services);

            var botStorage = this.ConfigureBotStorage();
            services.AddSingleton(botStorage);

            var dbConnStr = this.Configuration.GetConnectionString("BomberjamContext");

            this.ConfigureDatabase(services, dbConnStr);

            services.AddScoped<IBomberjamRepository, DatabaseRepository>();

            ConfigureHangfire(services, dbConnStr);

            // Google Analytics
            services.Configure<GoogleAnalyticsOptions>(options => Configuration.GetSection("GoogleAnalytics").Bind(options));
            services.AddTransient<ITagHelperComponent, GoogleAnalyticsTagHelperComponent>();
        }

        private void ConfigureDatabase(IServiceCollection services, string dbConnStr) => services.AddDbContext<BomberjamContext>(options =>
        {
            var dbBuilder = options.UseSqlServer(dbConnStr);

            if (this.Environment.IsDevelopment())
            {
                dbBuilder.EnableSensitiveDataLogging();
            }
        });

        private static void ConfigureHangfire(IServiceCollection services, string dbConnStr)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(dbConnStr);
            });

            services.AddHangfireServer();
        }

        private IBomberjamStorage ConfigureBotStorage()
        {
            var storages = new IBomberjamStorage[]
            {
                new LocalFileBomberjamStorage(Path.GetTempPath()),
                new AzureStorageBomberjamStorage(this.Configuration.GetConnectionString("BomberjamStorage"))
            };

            return new CompositeBomberjamStorage(storages);
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
                })
                .AddGitHub(options =>
                {
                    options.ClientId = this.Configuration["GitHub:ClientId"];
                    options.ClientSecret = this.Configuration["GitHub:ClientSecret"];
                    options.Scope.Add("user:email");
                    options.CallbackPath = "/signin-github-callback";
                })
                .AddSecret(this.Configuration["SecretAuth:Secret"]);
        }

        public void Configure(IApplicationBuilder app, IRecurringJobManager recurringJobs, ILogger<Startup> logger, GitHubConfiguration gitHubConfiguration)
        {
            app.UseResponseCompression();

            if (this.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Web/Error");
                app.UseHsts();
            }

            // Required to serve files with no extension in the .well-known folder
            var options = new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            };

            app.UseHttpsRedirection();
            app.UseStaticFiles(options);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHangfireDashboard(new DashboardOptions
                {
                    Authorization = new[] { new HangfireDashboardAuthorizationFilter(gitHubConfiguration) }
                });
            });

            var matchmakingCronExpr = this.Configuration["JobCrons:Matchmaking"];
            var orphanedTasksCronExpr = this.Configuration["JobCrons:OrphanedTasks"];

            logger.Log(LogLevel.Information, "Matchmaking cron expression: " + matchmakingCronExpr);
            logger.Log(LogLevel.Information, "Orphaned tasks cron expression: " + orphanedTasksCronExpr);

            recurringJobs.AddOrUpdate<MatchmakingJob>("matchmaking", job => job.Run(), matchmakingCronExpr);
            recurringJobs.AddOrUpdate<OrphanedTaskFixingJob>("orphanedTasks", job => job.Run(), orphanedTasksCronExpr);
        }
    }
}
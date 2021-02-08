using System.IO;
using System.Runtime.InteropServices;
using Bomberjam.Website.Authentication;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Storage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            var mvcBuilder = services.AddControllersWithViews();

            if (this.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
                })
                .AddGitHub(options =>
                {
                    options.ClientId = Configuration["GitHub:ClientId"];
                    options.ClientSecret = Configuration["GitHub:ClientSecret"];
                    options.Scope.Add("user:email");
                    options.CallbackPath = "/signin-github-callback";
                })
                .AddSecret(Configuration["SecretAuth:Secret"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bomberjam API", Version = "v1" });
            });

            var fileBotStorage = new LocalFileBotStorage(Path.GetTempPath());
            services.AddSingleton<IBotStorage>(fileBotStorage);
            services.AddSingleton<IObjectCache, ObjectCache>();

            var dbConnName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "BomberjamContextWin" : "BomberjamContextLin";
            services.AddDbContext<BomberjamContext>(options =>
            {
                options.UseSqlite(this.Configuration.GetConnectionString(dbConnName)).EnableSensitiveDataLogging();
            });

            services.AddScoped<DatabaseRepository>();
            services.AddScoped<IRepository, CachedRepository>(container =>
            {
                var repository = container.GetService<DatabaseRepository>();
                var objectCache = container.GetService<IObjectCache>();
                return new CachedRepository(repository, objectCache);
            });

            var zippedBotFileStream = typeof(Startup).Assembly.GetManifestResourceStream("Bomberjam.Website.MyBot.zip");
            if (zippedBotFileStream != null)
            {
                using (zippedBotFileStream)
                using (var zippedBotFileMs = new MemoryStream())
                {
                    zippedBotFileStream.CopyTo(zippedBotFileMs);
                    var zippedBotFileBytes = zippedBotFileMs.ToArray();

                    fileBotStorage.UploadBotSourceCode(Constants.UserAskaiserId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    fileBotStorage.UploadBotSourceCode(Constants.UserFalgarId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    fileBotStorage.UploadBotSourceCode(Constants.UserXenureId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    fileBotStorage.UploadBotSourceCode(Constants.UserMintyId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    fileBotStorage.UploadBotSourceCode(Constants.UserKalmeraId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    fileBotStorage.UploadBotSourceCode(Constants.UserPandarfId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                    fileBotStorage.UploadBotSourceCode(Constants.UserMireId, new MemoryStream(zippedBotFileBytes)).GetAwaiter().GetResult();
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (this.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bomberjam API"));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
            });
        }
    }
}

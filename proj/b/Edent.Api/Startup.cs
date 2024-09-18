using Edent.Api.Infrastructure;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Extensions;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Models;
using Edent.Api.Models.PlayMobile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.IO;

namespace Edent.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            GlobalConfiguration.WebRoot = _hostingEnvironment.WebRootPath;
            GlobalConfiguration.ContentRoot = _hostingEnvironment.ContentRootPath;
            services.AddOptions();
            services.AddLogging();
            services.AddCustomDbContext(Configuration);
            services.ConfigureCors("CorsPolicy");
            services.ConfigureIdentity();
            services.ConfigureAuthentication(Configuration);
            services.Configure<SmtpProvider>(Configuration.GetSection("SmtpProvider"));
            services.Configure<PlayMobileProvider>(Configuration.GetSection("PlayMobileProvider"));

            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                //auth.AddPolicy(PolicyConstants.DEFAULT, policy => policy.RequireClaim(CustomClaimTypes.Permission, PermissionConstants.DEFAULT));
            });
            services.AddAutoMapper(config =>
            {
                config.AddProfile(new ApplicationMappingProfile());
            });
            services.AddHttpContextAccessor();
            services.AddAppServices();
            services.AddHttpClient();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(ValidateModelStateFilter));

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Culture = CultureInfo.InvariantCulture;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.String;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            GlobalConfiguration.IsDevelopment = env.IsDevelopment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 &&
                   !Path.HasExtension(context.Request.Path.Value) &&
                   !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

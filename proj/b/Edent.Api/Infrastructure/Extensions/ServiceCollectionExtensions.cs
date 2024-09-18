using Edent.Api.Infrastructure.Auth;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Edent.Api.Infrastructure.Data.Seed;
using Edent.Api.Services.Notification.PlayMobile;
using Edent.Api.Services.Notification.Interfaces;
using Edent.Api.Services.Notification;
using Edent.Api.Services.Notification.Smtp;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.Services.EntityServices.Implementations;
using Edent.Api.Infrastructure.Resolvers;

namespace Edent.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql()
               .AddDbContext<ApplicationDbContext>((sp, options) =>
               {

                   options.UseInternalServiceProvider(sp);
                   options.UseNpgsql(
                       configuration.GetConnectionString("DefaultConnection"),
                        o =>
                        {
                            o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            o.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            o.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                        });
               }).BuildServiceProvider();
        }

        public static void ConfigureCors(this IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("*")
                    .AllowCredentials());
            });
        }
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequiredLength = 6;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase = false;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<ITokenFactory, TokenFactory>();
            services.AddSingleton<IJwtTokenHandler, JwtTokenHandler>();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Authentication:Jwt:Key"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["Authentication:Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = config["Authentication:Jwt:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.IncludeErrorDetails = true;
                o.TokenValidationParameters = tokenValidationParameters;
                o.SaveToken = true;
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    }
                };
            });
            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IDataInitializer, DataInitializer>();

            services.AddScoped(typeof(ISmsSender<>), typeof(SmsSender<>));
            services.AddScoped(typeof(IEmailSender<>), typeof(EmailSender<>));

            services.AddScoped(typeof(PlayMobileSmsSender));
            services.AddScoped(typeof(SmtpSender));
            services.AddScoped(typeof(UrlResolver));


            services.AddScoped<ISendSmsBehavior<PlayMobileSmsSender>>(x => x.GetService<PlayMobileSmsSender>());
            services.AddScoped<ISendEmailBehavior<SmtpSender>>(x => x.GetService<SmtpSender>());

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IEntityService<>), typeof(EntityService<>));

            services.AddTransient<IUserResolverService, UserResolverService>();
            services.AddTransient<IAuthRequestService, AuthRequestService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IPatientToothService, PatientToothService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IPatientAddressService, PatientAddressService>();
            services.AddTransient<ISpecializationService, SpecializationService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IScheduleSettingService, ScheduleSettingService>();
            services.AddTransient<ISettingDayOfWeekService, SettingDayOfWeekService>();
            services.AddTransient<ITermService, TermService>();
            services.AddTransient<IMeasurementUnitService, MeasurementUnitService>();
            services.AddTransient<IMeasurementUnitTypeService, MeasurementUnitTypeService>();
            services.AddTransient<IInventoryPriceService, InventoryPriceService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IDentalServiceService, DentalServiceService>();
            services.AddTransient<IDentalServiceCategoryService, DentalServiceCategoryService>();
            services.AddTransient<IDentalServiceGroupService, DentalServiceGroupService>();
            services.AddTransient<IDentalServicePriceService, DentalServicePriceService>();
            services.AddTransient<IDiagnoseService, DiagnoseService>();
            services.AddTransient<IProfessionService, ProfessionService>();
            services.AddTransient<IReceptInventorySettingItemService, ReceptInventorySettingItemService>();
            services.AddTransient<IReceptInventorySettingService, ReceptInventorySettingService>();
            services.AddTransient<IToothService, ToothService>();
            services.AddTransient<IDentalServiceReceptInventorySettingService, DentalServiceReceptInventorySettingService>();
            services.AddTransient<IDoctorInTermService, DoctorInTermService>();
            services.AddTransient<IDoctorAddressService, DoctorAddressService>();
            services.AddTransient<IPartnerService, PartnerService>();
            services.AddTransient<ITreatmentService, TreatmentService>();
            services.AddTransient<ITreatmentDentalServiceService, TreatmentDentalServiceService>();
            services.AddTransient<IReceptService, ReceptService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IIncomeInventoryItemService, IncomeInventoryItemService>();
            services.AddTransient<IOutcomeInventoryItemService, OutcomeInventoryItemService>();
            services.AddTransient<IInventoryIncomeService, InventoryIncomeService>();
            services.AddTransient<IInventoryOutcomeService, InventoryOutcomeService>();
            services.AddTransient<ITechnicService, TechnicService>();
            services.AddTransient<IJointDoctorService, JointDoctorService>();
            services.AddTransient<IDentalChairService, DentalChairService>();
            services.AddTransient<IDoctorDentalChairService, DoctorDentalChairService>();
            services.AddTransient<IReceptDentalServiceService, ReceptDentalServiceService>();

            return services;
        }
    }
}

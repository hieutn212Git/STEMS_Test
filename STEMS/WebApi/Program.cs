
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Business;
using Common;
using Common.MiddleWares;
using DataAccess.Context;
using Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Repository;
using Serilog;
using STEMS.MediatR;
using System.Reflection;
using System.Text;

namespace WebApi
{
    public static class Program
    {
        private static IConfiguration _configuration;


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Configuration(builder.Configuration);
            ConfigureServices(builder.Services);
            ConfigurePipelines(builder.Services);
            ConfigureHostBuilder(builder.Host);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void Configuration(ConfigurationManager configuration)
        {
            configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true, reloadOnChange: true);
            AppSettings.Instance = configuration.GetSection("AppSettings").Get<AppSettings>()!;

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .Enrich.WithProperty("@source", "STEMS");
            _configuration = configuration;

            Log.Logger = loggerConfig
                .WriteTo.Console()
                .CreateLogger();
        }

        private static void ConfigureHostBuilder(ConfigureHostBuilder host)
        {
            // Call UseServiceProviderFactory on the Host sub property 
            host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterInstance(Log.Logger);
                containerBuilder.RegisterModule<RepositoryModule>();
                containerBuilder.RegisterModule<BusinessModule>();
                containerBuilder.RegisterSelf();
            });
        }

        private static void RegisterSelf(this ContainerBuilder builder)
        {
            IContainer container = null;
            builder.Register(c => container).AsSelf().SingleInstance();
            builder.RegisterBuildCallback(c => container = c as IContainer);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllersWithViews();
            services
                .AddControllers()
                .ConfigureApiBehaviorOptions(o =>
                {
                    o.SuppressInferBindingSourcesForParameters = true;
                    o.SuppressModelStateInvalidFilter = true;
                });

            services.AddEndpointsApiExplorer();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(BaseValidator<>).GetTypeInfo().Assembly));
            services.AddHttpContextAccessor();
            services.AddHttpContext();
            services
            .AddAuthentication(opt =>
            {
                opt.DefaultScheme = AuthConstants.SCHEMES_BEARER_COOKIES;
                opt.DefaultChallengeScheme = AuthConstants.SCHEMES_BEARER_COOKIES;
            })
                .AddJwtBearer(AuthConstants.AUTH_SCHEME_BEARER, (JwtBearerOptions options) =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = AppSettings.Instance.Issuer,
                        ValidAudience = AppSettings.Instance.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddCookie(AuthConstants.AUTH_SCHEME_COOKIES, options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(_configuration["JwtSettings:AccessExpireMinutes"]));
                    options.SlidingExpiration = true;
                })
                .AddPolicyScheme(AuthConstants.SCHEMES_BEARER_COOKIES, AuthConstants.SCHEMES_BEARER_COOKIES, options =>
                {
                    // runs on each request
                    options.ForwardDefaultSelector = context =>
                    {
                        // filter by auth type
                        string authorization = context.Request.Headers[HeaderNames.Authorization];
                        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer"))
                            return AuthConstants.AUTH_SCHEME_BEARER;

                        // otherwise always check for cookie auth
                        return AuthConstants.AUTH_SCHEME_COOKIES;
                    };
                }); ;

            //services
            //    .AddAuthorizationBuilder()
            //    .SetDefaultPolicy(new AuthorizationPolicyBuilder()
            //            .RequireAuthenticatedUser()
            //            .AddAuthenticationSchemes("Bearer")
            //            .Build());

            services.AddRolePermissionAuthorize();
            services.AddAutoMapper(typeof(Program).Assembly);

            var configuration = new MapperConfiguration(cfg =>
            {
                // Map properties with a public getter
                cfg.ShouldMapProperty = pi => pi.GetMethod?.IsPublic == true;
                cfg.AddMaps(new[] { typeof(MapperConfig).Assembly });
            }
            );

            IMapper mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);



            services.AddDbContext<CoreDataContext>(options =>
            {
                options.UseSqlServer(AppSettings.Instance.CoreDb,
            sql =>
            {
                sql.EnableRetryOnFailure();
            });

#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });
            services.AddCors(o =>
            {
                o.AddPolicy("AllowSetOrigins", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.SetIsOriginAllowed(_ => true);
                    options.AllowCredentials();
                });
            });
        }

        private static void ConfigurePipelines(IServiceCollection services)
        {
            services.AddSingleton<ExceptionMiddleware>();
            services.AddValidatorsFromAssembly(typeof(BaseValidator<>).Assembly);
        }
    }
}

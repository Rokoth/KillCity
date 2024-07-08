using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KillCity.Common;
using KillCity.Db.Context;
using KillCity.Db.Interface;
using KillCity.Db.Repository;
using KillCity.KillCityDeployer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KillCity.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CommonOptions>(Configuration);
            services.AddControllersWithViews();
            services.AddLogging();
            services.AddSingleton<IErrorNotifyService, ErrorNotifyService>();
            services.AddDbContextPool<DbPgContext>((opt) =>
            {
                opt.EnableSensitiveDataLogging();
                var connectionString = Configuration.GetConnectionString("MainConnection");
                opt.UseNpgsql(connectionString);
            });

            services.AddCors();
            services.AddAuthentication()
            .AddJwtBearer("Token", options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //// ��������, ����� �� �������������� �������� ��� ��������� ������
                    ValidateIssuer = true,
                    //// ������, �������������� ��������
                    ValidIssuer = AuthOptions.ISSUER,

                    //// ����� �� �������������� ����������� ������
                    ValidateAudience = true,
                    //// ��������� ����������� ������
                    ValidAudience = AuthOptions.AUDIENCE,
                    //// ����� �� �������������� ����� �������������
                    ValidateLifetime = true,

                    // ��������� ����� ������������
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // ��������� ����� ������������
                    ValidateIssuerSigningKey = true,

                };
            }).AddCookie("Cookies", options => {
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = new PathString("/Account/Logout");
            });

            services
                .AddAuthorization(options =>
                {
                    var cookiePolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("Cookies")
                        .Build();
                    options.AddPolicy("Cookie", cookiePolicy);
                    options.AddPolicy("Token", new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("Token")
                        .Build());
                    options.DefaultPolicy = cookiePolicy;
                });

            services.AddScoped<IRepository<Db.Model.User>, Repository<Db.Model.User>>();
            services.AddScoped<IRepository<Db.Model.UserHistory>, Repository<Db.Model.UserHistory>>();
            //services.AddDataServices();
            services.AddScoped<IDeployService, DeployService>();
            //services.AddScoped<INotifyService, NotifyService>();
            services.ConfigureAutoMapper();
            services.AddSwaggerGen(swagger =>
            {
                //s.OperationFilter<AddRequiredHeaderParameter>();

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
           
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSwagger(opt=> { });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }

    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "access token",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("Bearer ")
                }
            });
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Db.Model.User, Contract.Model.User>();

            //CreateMap<Contract.Model.UserCreator, Db.Model.User>()
            //    .ForMember(s => s.Password, s => s.Ignore());

            //CreateMap<Db.Model.UserHistory, Contract.Model.UserHistory>();

            //CreateMap<Contract.Model.UserUpdater, Db.Model.User>()
            //    .ForMember(s => s.Password, s => s.Ignore());

            //CreateMap<Db.Model.Client, Contract.Model.Client>();

            //CreateMap<Contract.Model.ClientCreator, Db.Model.Client>()
            //    .ForMember(s => s.Password, s => s.Ignore());

            //CreateMap<Db.Model.ClientHistory, Contract.Model.ClientHistory>();

            //CreateMap<Contract.Model.ClientUpdater, Db.Model.Client>()
            //    .ForMember(s => s.Password, s => s.Ignore());

            //CreateMap<Db.Model.Release, Contract.Model.Release>();
            //CreateMap<Contract.Model.ReleaseCreator, Db.Model.Release>();
            //CreateMap<Contract.Model.ReleaseUpdater, Db.Model.Release>();

            //CreateMap<Db.Model.ReleaseArchitect, Contract.Model.ReleaseArchitect>();
            //CreateMap<Contract.Model.ReleaseArchitectCreator, Db.Model.ReleaseArchitect>();
            //CreateMap<Contract.Model.ReleaseArchitectUpdater, Db.Model.ReleaseArchitect>();

            //CreateMap<Db.Model.LoadHistory, Contract.Model.LoadHistory>();
            //CreateMap<Contract.Model.LoadHistoryCreator, Db.Model.LoadHistory>();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder)
        {
            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("MainConnection");
            builder.AddConfigDbProvider(options => options.UseNpgsql(connectionString), connectionString);
            return builder;
        }

        public static IConfigurationBuilder AddConfigDbProvider(
            this IConfigurationBuilder configuration, Action<DbContextOptionsBuilder> setup, string connectionString)
        {
            configuration.Add(new ConfigDbSource(setup, connectionString));
            return configuration;
        }

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new AutoMapper.MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static ILoggingBuilder AddErrorNotifyLogger(
        this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, ErrorNotifyLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <ErrorNotifyLoggerConfiguration, ErrorNotifyLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddErrorNotifyLogger(
            this ILoggingBuilder builder,
            Action<ErrorNotifyLoggerConfiguration> configure)
        {
            builder.AddErrorNotifyLogger();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}

using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.RabbitMQ;
using ApplicationCore.Models;
using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebAPI
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
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.WithOrigins("http://localhost:4200", "http://localhost:4201").AllowCredentials().AllowAnyHeader().AllowAnyMethod());
            });
            // use real database
            // Requires LocalDB which can be installed with SQL Server Express 2016
            // https://www.microsoft.com/en-us/download/details.aspx?id=54284
            services.AddDbContext<NoisContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

            //Settings
            var settings = new Settings();
            Configuration.Bind(nameof(settings), settings);
            services.AddSingleton(settings);

            //JwtSettings
            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            //RabbitMQSettings
            var rabbitMQSettings = new RabbitMQSettings();
            Configuration.Bind(nameof(rabbitMQSettings), rabbitMQSettings);
            services.AddSingleton(rabbitMQSettings);

            //SendGridConfigure
            var sendGridConfigure = new SendGridConfigure();
            Configuration.Bind(nameof(sendGridConfigure), sendGridConfigure);
            services.AddSingleton(sendGridConfigure);

            //add identity
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<NoisContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            //AddAuthentication
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = tokenValidationParameters;
            });
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = true;
                //options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                // Default User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
            services.AddScoped<DbContext, NoisContext>();
            services.AddScoped<UserManager<User>, UserManager<User>>();
            services.AddScoped<RoleManager<Role>, RoleManager<Role>>();
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(EfRepository<>));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddSingleton(typeof(IRabbitMQService), typeof(RabbitMQService));
            #region Configure for Repositories
            var allRepositoryInterfaces = Assembly.GetAssembly(typeof(IRepositoryAsync<>))
                .GetTypes().Where(t => t.Name.EndsWith("Repository")).ToList();
            var allRepositoryImplements = Assembly.GetAssembly(typeof(EfRepository<>))
                .GetTypes().Where(t => t.Name.EndsWith("Repository")).ToList();

            foreach (var repositoryType in allRepositoryInterfaces.Where(t => t.IsInterface))
            {
                var implement = allRepositoryImplements.FirstOrDefault(c => c.IsClass && repositoryType.Name.Substring(1) == c.Name);
                if (implement != null) services.AddTransient(repositoryType, implement);
            }
            #endregion Configure for Repositories
            #region Configure for Serivces
            var allServicesInterfaces = Assembly.GetAssembly(typeof(IService))
                .GetTypes().Where(t => t.Name.EndsWith("Service")).ToList();
            var allServiceImplements = Assembly.GetAssembly(typeof(Service))
                .GetTypes().Where(t => t.Name.EndsWith("Service")).ToList();

            foreach (var serviceType in allServicesInterfaces.Where(t => t.IsInterface))
            {
                var implement = allServiceImplements.FirstOrDefault(c => c.IsClass && serviceType.Name.Substring(1) == c.Name);
                if (implement != null) services.AddTransient(serviceType, implement);
            }

            #endregion Configure for Serivces

            //services.AddControllers(options =>
            //{
            //    options.Filters.Add<ValidationFilter>();
            //}).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddControllers();
            //Register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TIGTAG API",
                    Description = "ASP.NET Core Web API For TIGTAG Project",
                    TermsOfService = new Uri("https://tigtag.vn"),
                    Contact = new OpenApiContact
                    {
                        Name = "Tigtag",
                        Email = string.Empty,
                        Url = new Uri("https://tigtag.vn"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Tigtag",
                        Url = new Uri("https://tigtag.vn"),
                    }
                });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //Hangfire service
            //HangfireService(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowOrigin");
            //app.UseHangfireDashboard();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = "swagger";
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStatusCodePagesWithReExecute("/"); // <-- added to redirect to angular
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        #region Hangfire service
        //private void HangfireService(IServiceCollection services)
        //{
        //    services.AddHangfire(configuration => configuration
        //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        //    .UseSimpleAssemblyNameTypeSerializer()
        //    .UseRecommendedSerializerSettings()
        //    .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
        //    {
        //        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        //        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        //        QueuePollInterval = TimeSpan.Zero,
        //        UseRecommendedIsolationLevel = true,
        //        UsePageLocksOnDequeue = true,
        //        DisableGlobalLocks = true
        //    }));

        //    var sqlStorage = new SqlServerStorage(Configuration.GetConnectionString("HangfireConnection"));
        //    JobStorage.Current = sqlStorage;

        //    // Add the processing server as IHostedService
        //    services.AddHangfireServer();

        //    var serviceProvider = services.BuildServiceProvider();
        //    var _hangfireService = serviceProvider.GetService<IHangfireService>();

        //    //Use method in hangfire service
        //    _hangfireService.Start();
        //}
        #endregion

    }
}
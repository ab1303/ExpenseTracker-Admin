using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Admin.Repositories;
using Admin.Services;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;
using Swashbuckle.AspNetCore.Swagger;
using Admin.Api.Extensions;
using Admin.Api.Middleware.Logging;
using Admin.Api.SwashBuckle;
using Admin.Common;
using Admin.Infrastructure.HttpClients;
using Admin.Infrastructure.Logging;
using Admin.Infrastructure.Logging.SerilogAdaptor;
using Admin.Infrastructure.Logging.SerilogAdaptor.interfaces;
using Admin.Repositories.DbContext;
using Admin.Services.interfaces;
using Admin.Services.Results;

namespace Admin.Api
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
            services.AddAutoMapper(typeof(Startup));

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpClient<IBslApiClient, BslApiClient>();

            // Singletons
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<IRepositories, Admin.Repositories.Repositories>();

            // Register Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserVerificationStatusService, UserVerificationStatusService>();

            // SeriLogger Setup
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.ControlledBy(new EnvironmentVariableLoggingLevelSwitch())
                .CreateLogger();

            services.AddSingleton<ILogger, Serilog.Core.Logger>(provider => logger);
            services.AddSingleton<ILogContextService, SerilogLogContextService>();

            var connectionString = string.IsNullOrEmpty(Configuration["AdminDB:ConnectionString"]) 
                ? StackVariables.UserDbConnString
                : Configuration["AdminDB:ConnectionString"]
                ;

            if (!string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<AdminDbContext>(options => options.UseMySql(connectionString, mysqlOptions =>
                {
                    mysqlOptions.MigrationsAssembly("User.Repositories");
                }));
            }
            else
            {
                logger.Error("Unable to retrieve connection string for AdminDB. Exiting Application");
                Environment.Exit(1);
            }

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                {
                    var message = string.Join(" | ", actionContext.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return new BadRequestObjectResult(new Error { ErrorCode = ErrorCodes.RequestInvalidGeneric, SystemMessage = message });
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.ApiVersion.V1, new Info
                {
                    Version = Constants.ApiVersion.V1,
                    Title = Configuration["ProjectsDetails:Title"],
                    Description = Configuration["ProjectsDetails:Description"],
                    Contact = new Contact
                    {
                        Name = Configuration["ProjectsDetails:ContactName"]
                    }
                });
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger logger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration["ProjectsDetails:DocumentationTitle"]);
                c.RoutePrefix = string.Empty;
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWhen(context => !context.Request.Path.Value.EndsWith("healthcheck"), appBuilder =>
            {
                appBuilder.UseMiddleware<LoggingMiddleware>();
            });

            app.ConfigureExceptionHandler(logger);

            app.UseMvc();

            // TODO: Uncomment if required
            //EnsureMigrationOfContext<AdminDbContext>(app);
        }

        private void EnsureMigrationOfContext<T>(IApplicationBuilder app) where T : DbContext
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<T>();
                context.Database.Migrate();
            }
        }
    }
}

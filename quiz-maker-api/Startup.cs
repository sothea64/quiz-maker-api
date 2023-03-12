using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using quiz_maker_api.Helpers;
using System.Linq;
using quiz_maker_api.Validations;
using FluentValidation.AspNetCore;

namespace quiz_maker_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            if (_env.IsProduction())
            {
                services.AddLogging(config =>
                {
                    config.ClearProviders();
                    config.AddDebug();
                    config.AddFilter<ConsoleLoggerProvider>(logLevel => logLevel >= LogLevel.Warning);
                    config.AddConsole();
                });
            }
            else
            {
                services.AddLogging(config =>
                {
                    config.ClearProviders();
                    config.AddDebug();
                    config.AddFilter<ConsoleLoggerProvider>(logLevel => logLevel >= LogLevel.Information);
                    config.AddConsole();
                });
            }

            // reading setting from appsetting.json and environment variables.
            services.Configure<AppSetting>(Configuration.GetSection("App"));

            //Get setting from service
            var setting = services.BuildServiceProvider().GetService<IOptions<AppSetting>>().Value;

            // database configuration.
            services.AddEntityFrameworkSqlServer();
            services.AddDbContextPool<QuizMakerDbContext>(options => options.UseSqlServer(
                setting.ConnectionString), 1024);

            // allow user ._current.UserId or ._current.BranchId
            // inside Logics. sinc CurrentScope is required HttpContext
            // so we need to inject HttpContextAccessor also.
            services.AddHttpContextAccessor();

            // swagger doc register.
            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "QUIZ MAKER API";
                document.Title = "QUIZ MAKER API";
                document.Version = "v1";
                document.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header
                });
                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("bearer"));
            });

            services.AddScoped<ICurrentScope, HttpCurrentScope>();

            //Inject all logics in project
            services.InjectAllLogics();

            var key = AppSetting.SECRET_KEY;

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    // 2017-01-25T07:00:00Z             UTC 
                    // 2017-01-25T07:00:00              Unspecified
                    // 2017-01-25T07:00:00+07:00:00     Local GMT+7 PhnomPenh
                    ////options.JsonSerializerOptions.D = DateTimeZoneHandling.Unspecified;
                })
                .AddFluentValidation(
                        f => f.RegisterValidatorsFromAssemblyContaining<QuizzesValidation>()
                );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            
            app.UseStaticFiles();
            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                var redirectRootToSwagger = new RewriteOptions().AddRedirect("^$", "swagger");
                app.UseRewriter(redirectRootToSwagger);
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }
            else
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("QUIZ MAKER API WORKED ... :)"); // returns a 200 with "API" as content.
                });
            }
        }
    }
}

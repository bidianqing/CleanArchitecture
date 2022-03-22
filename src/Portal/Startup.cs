using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Portal.Infrastructure;
using StackExchange.Redis;
using System.Net.Mime;
using System.Reflection;

namespace Portal
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
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = (actionContext) =>
                    {
                        actionContext.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                        string errorMessage = actionContext.ModelState.Values.First(u => u.Errors.Count > 0).Errors.First().ErrorMessage;

                        JObject obj = new JObject();
                        obj["success"] = false;
                        obj["message"] = errorMessage;
                        obj["data"] = null;

                        return new JsonResult(obj);
                    };
                })
                .AddNewtonsoftJson(options =>
                {

                });

            services.AddHttpContextAccessor();

            services.AddMemoryCache();

            services.AddDapper(options =>
            {
                options.ConnectionString = "";
                options.DatabaseType = DatabaseType.MySql;
            });

            services.AddTransient<CustomLoggingHttpMessageHandler>();
            services.AddHttpClient("common").AddHttpMessageHandler<CustomLoggingHttpMessageHandler>();

            // Register the Swagger generator, defining one or more Swagger documents
            // https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/getting-started-with-swashbuckle
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Portal.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Domain.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Infrastructure.xml"), true);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = HeaderNames.Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = "在下框中输入Bearer 你的jwt token",
                });

                c.OperationFilter<BearerAuthOperationsFilter>();
            });

            services.AddAllRegisterTypes(Assembly.Load("Domain"), Assembly.Load("Infrastructure"));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });
            // or
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var connection = ConnectionMultiplexer.Connect("localhost:6379");

                var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();
                connection.HashSlotMoved += (sender, e) =>
                {
                    logger.LogWarning("HashSlotMoved");
                };
                connection.ConfigurationChangedBroadcast += (sender, e) =>
                {
                    logger.LogWarning("ConfigurationChangedBroadcast");
                };
                connection.ErrorMessage += (sender, e) =>
                {
                    logger.LogWarning("ErrorMessage");
                };
                connection.ConnectionFailed += (sender, e) =>
                {
                    logger.LogWarning("ConnectionFailed");
                };
                connection.InternalError += (sender, e) =>
                {
                    logger.LogWarning("InternalError");
                };
                connection.ConnectionRestored += (sender, e) =>
                {
                    logger.LogWarning("ConnectionRestored");
                };
                connection.ConfigurationChanged += (sender, e) =>
                {
                    logger.LogWarning("ConfigurationChanged");
                };

                return connection;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = async (httpContext) =>
                {
                    var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();
                    var ex = httpContext.Features.Get<IExceptionHandlerFeature>();
                    logger.LogError(ex?.Error, "程序异常");

                    httpContext.Response.ContentType = MediaTypeNames.Application.Json;

                    JObject obj = new JObject();
                    obj["success"] = false;
                    obj["message"] = "服务器开小差了，稍后再试吧";
                    obj["data"] = null;

                    await httpContext.Response.WriteAsync(obj.ToString());
                }
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

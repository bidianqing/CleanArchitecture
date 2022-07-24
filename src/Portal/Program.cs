using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = (actionContext) =>
                    {
                        actionContext.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                        string errorMessage = actionContext.ModelState.Values.First(u => u.Errors.Count > 0).Errors.First().ErrorMessage;

                        return new JsonResult(ResultModel.Fail<object>(message: errorMessage));
                    };
                })
                .AddNewtonsoftJson(options =>
                {

                });

builder.Services.AddMediatR(Assembly.Load("Domain"), Assembly.Load("Portal"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

builder.Services.AddDapper(options =>
{
    options.ConnectionString = "server=127.0.0.1;port=3306;database=demo;user id=root;password=root;CharacterSet=utf8mb4;SslMode=None;Allow User Variables=true;";
    options.DatabaseType = DatabaseType.MySql;
});

builder.Services.AddTransient<CustomLoggingHttpMessageHandler>();
builder.Services.AddHttpClient("default").AddHttpMessageHandler<CustomLoggingHttpMessageHandler>();

// Register the Swagger generator, defining one or more Swagger documents
// https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/getting-started-with-swashbuckle
builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddAllRegisterTypes(Assembly.Load("Portal"), Assembly.Load("Domain"), Assembly.Load("Infrastructure"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});
// or
builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    var connection = ConnectionMultiplexer.Connect("localhost:6379");

    var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
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

var app = builder.Build();
var logger = app.Logger;

// Configure the HTTP request pipeline.
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandler = async (httpContext) =>
    {
        var ex = httpContext.Features.Get<IExceptionHandlerFeature>();
        logger.LogError(ex?.Error, "OnException");

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        await httpContext.Response.WriteAsync(ResultModel.Fail<object>(message: "服务器开小差了，请稍后再试！").ToString());
    }
});

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
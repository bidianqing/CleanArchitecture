using Infrastructure;
using MediatR;
using SqlSugar;

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
                });
builder.Services.AddMediatR(xfg =>
{
    xfg.RegisterServicesFromAssemblies(Assembly.Load("Domain"), Assembly.Load("Portal"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ISqlSugarClient>(sp =>
{
    var loggerFactory = sp.GetService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<ISqlSugarClient>();

    var configs = new List<ConnectionConfig>
    {
        new ConnectionConfig
        {
            ConfigId = "default",
            DbType = DbType.MySql,
            ConnectionString = "server=127.0.0.1;port=3306;database=demo;user id=root;password=root;CharacterSet=utf8mb4;SslMode=None;Allow User Variables=true;AllowPublicKeyRetrieval=true",
            IsAutoCloseConnection = true,
            AopEvents = new AopEvents
            {
                OnLogExecuting = (sql, parameters) =>
                {
                    
                },
                OnLogExecuted = (sql, parameters) =>
                {
                    
                },
                DataExecuting = (obj, model) =>
                {
                    
                },
            },
        }
    };

    var db = new SqlSugarClient(configs);

    db.Aop.OnLogExecuted = (sql, parameters) =>
    {
        var executedTime = db.Ado.SqlExecutionTime.TotalMilliseconds;
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Sql Executed in {time} ms \r\n {sql}", executedTime, UtilMethods.GetSqlString(DbType.SqlServer, sql, parameters));
        }

        //执行时间超过1秒
        if (executedTime > 1000 && logger.IsEnabled(LogLevel.Warning))
        {
            logger.LogWarning("Sql Executed in {time} ms \r\n {sql}", executedTime, UtilMethods.GetSqlString(DbType.SqlServer, sql, parameters));
        }
    };

    return db;
});
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddTransient<CustomLoggingHttpMessageHandler>();
builder.Services.AddHttpClient("default").AddHttpMessageHandler<CustomLoggingHttpMessageHandler>();


builder.Services.AddAllRegisterTypes(Assembly.Load("Portal"), Assembly.Load("Domain"), Assembly.Load("Infrastructure"));

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

app.MapControllers();

app.Run();
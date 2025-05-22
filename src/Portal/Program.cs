using Infrastructure.Repositories;
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

    var httpContextAccessor = sp.GetService<IHttpContextAccessor>();

    var configs = builder.Configuration.GetSection("DbConfigs").Get<ConnectionConfig[]>();

    var db = new SqlSugarClient([.. configs]);

    foreach (var config in configs)
    {
        db.GetConnection(config.ConfigId).Aop.OnLogExecuted = (sql, parameters) =>
        {
            var totalExecutedTime = db.Ado.SqlExecutionTime.TotalMilliseconds;
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Sql Executed in {time} ms \r\n{sql}", totalExecutedTime, UtilMethods.GetSqlString(config.DbType, sql, parameters));
            }

            if (totalExecutedTime > 1000)
            {
                logger.LogWarning("Sql Executed in {time} ms \r\n{sql}", totalExecutedTime, UtilMethods.GetSqlString(config.DbType, sql, parameters));
            }
        };

        db.GetConnection(config.ConfigId).Aop.DataExecuting = (oldValue, entityInfo) =>
        {
            if (entityInfo.OperationType == DataFilterType.InsertByObject)
            {
                // 主键Id生成策略
                if (entityInfo.PropertyName == nameof(BaseEntity.Id)
                    && entityInfo.EntityValue is BaseEntity baseEntity
                    && baseEntity.Id == Guid.Empty)
                {
                    entityInfo.SetValue(Guid.CreateVersion7());
                }

                // 创建时间、更新时间生成策略
                if (entityInfo.PropertyName == nameof(BaseAuditableEntity.Created) || entityInfo.PropertyName == nameof(BaseAuditableEntity.LastModified))
                {
                    entityInfo.SetValue(DateTimeOffset.Now);
                }

                // 创建人、更新人生成策略
                if (entityInfo.PropertyName == nameof(BaseAuditableEntity.CreatedBy) || entityInfo.PropertyName == nameof(BaseAuditableEntity.LastModifiedBy))
                {
                    entityInfo.SetValue(httpContextAccessor.HttpContext?.User.Identity.Name ?? "匿名用户");
                }
            }
            else if (entityInfo.OperationType == DataFilterType.UpdateByObject)
            {
                // 更新时间生成策略
                if (entityInfo.PropertyName == nameof(BaseAuditableEntity.LastModified))
                {
                    entityInfo.SetValue(DateTimeOffset.Now);
                }

                // 更新人生成策略
                if (entityInfo.PropertyName == nameof(BaseAuditableEntity.LastModifiedBy))
                {
                    entityInfo.SetValue(httpContextAccessor.HttpContext?.User.Identity.Name ?? "匿名用户");
                }
            }
        };
    }


    return db;
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

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
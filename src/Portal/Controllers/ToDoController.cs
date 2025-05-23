using Domain.AggregatesModel.ToDoAggregate;
using MediatR;
using Portal.Application.Commands;
using Portal.Application.Queries;

namespace Portal.Controllers
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public class TodoServices(
        IMediator mediator,
        IToDoQueries queries,
        ILogger<TodoServices> logger)
    {
        public IMediator Mediator { get; set; } = mediator;
        public ILogger<TodoServices> Logger { get; } = logger;
        public IToDoQueries Queries { get; } = queries;
    }


    [ApiController]
    [Route("todo")]
    public class ToDoController : ControllerBase
    {
        private readonly TodoServices services;

        public ToDoController(TodoServices todoServices)
        {
            services = todoServices;
        }

        [HttpPost]
        public async Task<ResultModel<Guid>> Post([FromBody] CreateToDoCommand command)
        {
            Guid id = await services.Mediator.Send(command);

            return ResultModel.Success(id);
        }

        [HttpGet("{id}")]
        public async Task<ResultModel<ToDo>> Get([FromRoute] Guid id)
        {
            var todo = await services.Queries.Get(id);

            return ResultModel.Success(todo);
        }
    }
}

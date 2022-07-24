using Domain.AggregatesModel.ToDoAggregate;
using MediatR;
using Portal.Application.Commands;
using Portal.Application.Queries;

namespace Portal.Controllers
{
    [ApiController]
    [Route("todo")]
    public class ToDoController : ControllerBase
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly IMediator _mediator;
        private readonly IToDoQueries _toDoQueries;

        public ToDoController(ILogger<ToDoController> logger, IMediator mediator, IToDoQueries toDoQueries)
        {
            _logger = logger;
            _mediator = mediator;
            _toDoQueries = toDoQueries;
        }

        [HttpPost]
        public async Task<ResultModel<int>> Post([FromBody] CreateToDoCommand command)
        {
            var todoId =  await _mediator.Send(command);

            return ResultModel.Success(todoId);
        }

        [HttpGet("{id}")]
        public async Task<ResultModel<ToDo>> Get([FromRoute] int id)
        {
            var todo = await _toDoQueries.Get(id);

            return ResultModel.Success(todo);
        }
    }
}

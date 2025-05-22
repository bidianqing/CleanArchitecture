using Domain.AggregatesModel.OrderAggregate;
using Domain.AggregatesModel.ToDoAggregate;
using Domain.Events;
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
        public async Task<ResultModel<Guid>> Post([FromBody] CreateToDoCommand command)
        {
            Guid id = await _mediator.Send(command);

            return ResultModel.Success(id);
        }

        [HttpGet("{id}")]
        public async Task<ResultModel<ToDo>> Get([FromRoute] Guid id)
        {
            var todo = await _toDoQueries.Get(id);

            await _mediator.Publish(new CreatedOrderDomainEvent(new Order(), id));

            return ResultModel.Success(todo);
        }
    }
}

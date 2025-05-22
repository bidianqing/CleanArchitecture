using Domain.AggregatesModel.ToDoAggregate;
using MediatR;

namespace Portal.Application.Commands
{
    public class CreateToDoCommand : IRequest<Guid>
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class CreateToDoCommandHandler : IRequestHandler<CreateToDoCommand, Guid>
    {
        private readonly IToDoRepository _toDoRepository;

        public CreateToDoCommandHandler(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<Guid> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            var todo = new ToDo
            {
                Title = request.Title,
                Description = request.Description,
            };

            var insertedTodoEntity = await _toDoRepository.InsertReturnEntityAsync(todo, cancellationToken);

            return insertedTodoEntity.Id;
        }
    }
}

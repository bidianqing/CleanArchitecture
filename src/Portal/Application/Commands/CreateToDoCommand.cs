using Domain.AggregatesModel.ToDoAggregate;
using MediatR;

namespace Portal.Application.Commands
{
    public class CreateToDoCommand : IRequest<int>
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class CreateToDoCommandHandler : IRequestHandler<CreateToDoCommand, int>
    {
        private readonly IToDoRepository _toDoRepository;

        public CreateToDoCommandHandler(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<int> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            var todo = new ToDo
            {
                Title = request.Title,
                Description = request.Description,
                CreateTime = DateTime.Now
            };

            return await _toDoRepository.Add(todo);
        }
    }
}

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
        private readonly IRepository<ToDo> _toDoRepository;

        public CreateToDoCommandHandler(IRepository<ToDo> toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<int> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            var todo = new ToDo
            {
                Id = Guid.CreateVersion7(DateTimeOffset.Now),
                Title = request.Title,
                Description = request.Description,
                CreateTime = DateTime.Now,
                //UpdateTime = DateTime.Now,
                //IsDeleted = false,
                //Creator = "admin",
                //Updater = "admin",
                //DeletedTime = null,
                //Deleted = false,
                //DeletedBy = null,
                //Version = 1,
                //TenantId = "1",
            };

            return await _toDoRepository.Context.Insertable(todo).ExecuteReturnIdentityAsync();
        }
    }
}

using Domain.AggregatesModel.ToDoAggregate;

namespace Portal.Application.Queries
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IToDoQueries
    {
        Task<ToDo> Get(Guid id);
    }

    public class ToDoQueries : IToDoQueries
    {
        private readonly IToDoRepository _toDoRepository;
        public ToDoQueries(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<ToDo> Get(Guid id)
        {
            return await _toDoRepository.GetByIdAsync(id);
        }
    }
}

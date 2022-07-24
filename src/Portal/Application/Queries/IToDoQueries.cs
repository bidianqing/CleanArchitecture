using Domain.AggregatesModel.ToDoAggregate;

namespace Portal.Application.Queries
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IToDoQueries
    {
        Task<ToDo> Get(int id);
    }

    public class ToDoQueries : IToDoQueries
    {
        private readonly IToDoRepository _toDoRepository;
        public ToDoQueries(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<ToDo> Get(int id)
        {
            return await _toDoRepository.Get(id);
        }
    }
}

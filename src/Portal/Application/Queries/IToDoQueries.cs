using Domain.AggregatesModel.ToDoAggregate;
using Domain.SeedWork;

namespace Portal.Application.Queries
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IToDoQueries
    {
        Task<ToDo> Get(int id);
    }

    public class ToDoQueries : IToDoQueries
    {
        private readonly IRepository<ToDo> _toDoRepository;
        public ToDoQueries(IRepository<ToDo> toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<ToDo> Get(int id)
        {
            return await _toDoRepository.GetByIdAsync(id);
        }
    }
}

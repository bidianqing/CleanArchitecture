using Dapper.Contrib.Extensions;
using Domain.AggregatesModel.ToDoAggregate;

namespace Infrastructure.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        public ToDoRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> Add(ToDo todo)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.InsertAsync(todo);
        }

        public async Task<ToDo> Get(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.GetAsync<ToDo>(id);
        }
    }
}

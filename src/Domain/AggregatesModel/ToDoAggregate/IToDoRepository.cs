namespace Domain.AggregatesModel.ToDoAggregate
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IToDoRepository : IRepository<ToDo>
    {

    }
}

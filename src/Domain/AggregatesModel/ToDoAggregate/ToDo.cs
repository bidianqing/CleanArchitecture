using SqlSugar;

namespace Domain.AggregatesModel.ToDoAggregate
{
    [SugarTable("tb_todo")]
    public class ToDo : Entity<int>, IAggregateRoot
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreateTime { get; set; }
    }
}

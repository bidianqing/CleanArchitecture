using SqlSugar;

namespace Domain.AggregatesModel.ToDoAggregate
{
    [SugarTable("tb_todo")]
    public class ToDo : BaseAuditableEntity, IAggregateRoot
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}

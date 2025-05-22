using MediatR;
using System.Text.Json.Serialization;

namespace Domain.Events
{
    /// <summary>
    /// 1、事件的类名称应使用过去时态谓词表示
    /// 2、由于事件是在过去发生的，所以不应发生更改。 因此，它必须是一个不可变的类。 属性是只读的。 无法更新对象，只能在创建它时设置值。
    /// </summary>
    public class CreatedOrderDomainEvent : INotification
    {
        [JsonPropertyName("order")]
        public Order Order { get; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; }

        public CreatedOrderDomainEvent(Order order, Guid userId)
        {
            this.Order = order;
            this.UserId = userId;
        }
    }
}

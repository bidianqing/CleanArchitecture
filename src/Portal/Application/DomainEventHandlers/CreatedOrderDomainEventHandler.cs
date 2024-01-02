using Domain.Events;
using MediatR;

namespace Portal.Application.DomainEventHandlers
{
    public class CreatedOrderDomainEventHandler : INotificationHandler<CreatedOrderDomainEvent>
    {
        public async Task Handle(CreatedOrderDomainEvent notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync();
        }
    }
}

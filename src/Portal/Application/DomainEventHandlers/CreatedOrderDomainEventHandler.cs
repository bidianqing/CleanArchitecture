using Domain.Events;
using MediatR;
using System.Text.Json;

namespace Portal.Application.DomainEventHandlers
{
    public class CreatedOrderDomainEventHandler : INotificationHandler<CreatedOrderDomainEvent>
    {
        private readonly  ILogger<CreatedOrderDomainEventHandler> _logger;

        public CreatedOrderDomainEventHandler(ILogger<CreatedOrderDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(CreatedOrderDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(JsonSerializer.Serialize(notification));
            await Console.Out.WriteLineAsync("CreatedOrderDomainEventHandler");
        }
    }
}

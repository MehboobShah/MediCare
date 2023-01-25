using MediCare.Domain.Common.Events;

namespace MediCare.Application.Catalog.Products.EventHandlers;

public class ProductCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Product>>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Product> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}
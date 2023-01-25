using MediCare.Shared.Events;

namespace MediCare.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}
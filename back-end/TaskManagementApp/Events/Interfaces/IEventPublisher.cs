using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Models.Events;

namespace TaskManagementApp.Events.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEnumerable<ITaskEvent> events);
    }
}

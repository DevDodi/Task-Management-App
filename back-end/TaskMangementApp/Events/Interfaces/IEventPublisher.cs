using TaskMangementApp.Models.DTOs;
using TaskMangementApp.Models.Events;

namespace TaskMangementApp.Events.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEnumerable<ITaskEvent> events);
    }
}

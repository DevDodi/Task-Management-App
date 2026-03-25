using TaskMangementApp.Models.DTOs;
using TaskMangementApp.Models.Events;

namespace TaskMangementApp.Events.Interfaces
{
    public interface IEventHandler
    {
        bool CanHandle(ITaskEvent taskEvent);
        Task HandleAsync(ITaskEvent taskEvent);
    }
}

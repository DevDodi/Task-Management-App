using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Models.Events;

namespace TaskManagementApp.Events.Interfaces
{
    public interface IEventHandler
    {
        bool CanHandle(ITaskEvent taskEvent);
        Task HandleAsync(ITaskEvent taskEvent);
    }
}

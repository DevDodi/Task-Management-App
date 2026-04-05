using TaskManagementApp.Events.Interfaces;
using TaskManagementApp.Models.DTOs;

namespace TaskManagementApp.Events
{
    public class EventPublisher : IEventPublisher
    {

        private IEnumerable<IEventHandler> eventHandlers;

        public EventPublisher(IEnumerable<IEventHandler> eventHandlers)
        {
            this.eventHandlers = eventHandlers;
        }

        public async Task PublishAsync(IEnumerable<ITaskEvent> events)
        {
            foreach (var taskEvent in events)
            {
                foreach (var handler in eventHandlers)
                {
                    if (handler.CanHandle(taskEvent))
                       await handler.HandleAsync(taskEvent);
                }
            }
        }
    }
}

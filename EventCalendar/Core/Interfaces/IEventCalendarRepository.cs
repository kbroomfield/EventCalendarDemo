using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCalendar.Core.Models;

namespace EventCalendar.Core.Interfaces
{
    public interface IEventCalendarRepository
    {
        Task CreateEventCalendarAsync(EventCalendarO eventCalendar);
        Task<List<Event>> GetAllEventsAsync(string userName);
        Task AddEventAsync(Event newEvent);
        Task UpdateEventAsync(Event newEvent);
        Task<EventCalendarO> GetUserEventCalendarAsync(string userName);
        Task<Event> Find(int id, string userName);

    }
}

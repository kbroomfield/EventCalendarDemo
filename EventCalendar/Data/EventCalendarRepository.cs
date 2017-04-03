using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCalendar.Core.Interfaces;
using EventCalendar.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventCalendar.Data
{
    public class EventCalendarRepository: IEventCalendarRepository
    {
        private readonly CalendarDbContext _dbContext;

        public EventCalendarRepository(CalendarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  Task CreateEventCalendarAsync(EventCalendarO eventCalendar)
        {
            _dbContext.EventCalendars.Add(eventCalendar);
            return _dbContext.SaveChangesAsync();
        }

        public Task<EventCalendarO> GetUserEventCalendarAsync(string userName)
        {
            var calendars = from c in _dbContext.EventCalendars select c;

            return calendars.Where(cal => cal.EventCalendarUser == userName).FirstOrDefaultAsync();
        }

        public Task<List<Event>> GetAllEventsAsync(string userName)
        {
            var events = from e in _dbContext.Events select e;

            return events.AsNoTracking().Where(evt => evt.User == userName).ToListAsync();
        }
        
        public Task AddEventAsync(Event newEvent)
        {
            _dbContext.Events.Add(newEvent);

            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateEventAsync(Event newEvent)
        {
            _dbContext.Events.Update(newEvent);

            return _dbContext.SaveChangesAsync();
        }

        public Task<Event> Find(int id, string userName)
        {
            return _dbContext.Events.AsNoTracking().FirstOrDefaultAsync(e => e.EventId == id && e.User == userName);
        }
    }
}

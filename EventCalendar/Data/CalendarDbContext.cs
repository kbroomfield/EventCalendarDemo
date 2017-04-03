using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCalendar.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventCalendar.Data
{
    public class CalendarDbContext: IdentityDbContext<EventCalendarUser>
    {
        public DbSet<EventCalendarO> EventCalendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAttendee> EventAttendees { get; set; }

        public CalendarDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventCalendar.Core.Models
{
    public class EventDTO
    {
        // calendar, title, event date and time, location, attendee list, reminder time, and whether the reminder has been sent).
        [Required]
        public string Title { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int EventDay { get; set; }
        [Required]
        public int EventMonth { get; set; }
        [Required]
        public int EventYear { get; set; }

        public int ReminderDay { get; set; }
        public int ReminderMonth { get; set; }
        public int ReminderYear { get; set; }
        public List<EventAttendee> Attendees { get; set; }
    }
}

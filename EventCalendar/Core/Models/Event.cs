using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventCalendar.Core.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }
        [Required]
        public DateTime EventDateTime { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public string Title { get; set; }
        public List<EventAttendee> AttendeeList { get; set; } = new List<EventAttendee>();
        public DateTime? ReminderTime { get; set; }
        public bool ReminderSent { get; set; } = false;
    }
}

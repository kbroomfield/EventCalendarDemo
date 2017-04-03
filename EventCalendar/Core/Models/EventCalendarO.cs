using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventCalendar.Core.Models
{
    public class EventCalendarO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventCalendarId { get; set; }
        [Required]
        public string EventCalendarName { get; set; }
        [Required]
        public string EventCalendarUser { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();
    }
}

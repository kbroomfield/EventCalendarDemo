using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using EventCalendar.Core.Interfaces;
using EventCalendar.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventCalendar.Controllers
{
    [Produces("application/json")]
    [Route("api/events")]
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class EventsController : Controller
    {
        private readonly IEventCalendarRepository _calendarRepository;
        private readonly UserManager<EventCalendarUser> _userManager;

        public EventsController(IEventCalendarRepository calendarRepository, UserManager<EventCalendarUser> userManager)
        {
            _calendarRepository = calendarRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            var events = await _calendarRepository.GetAllEventsAsync(user.UserName);

            return Ok(events);
        }

        [HttpGet("{id}", Name = "GetEvent")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            var foundEvent = _calendarRepository.Find(id, user.UserName);

            if (foundEvent == null)
            {
                return NotFound();
            }

            return Ok(foundEvent);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] EventDTO eventDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null )
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event newEvent = new Event
            {
                User = user.UserName,
                AttendeeList = eventDTO.Attendees,
                EventDateTime = new DateTime(eventDTO.EventYear, eventDTO.EventMonth, eventDTO.EventDay),
                Location = eventDTO.Location,
                Title = eventDTO.Title,
                ReminderSent = false
            };

            await _calendarRepository.AddEventAsync(newEvent);

            return Created("/events/id", eventDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updateEvent)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid || updateEvent.User != user.UserName || id != updateEvent.EventId)
            {
                return BadRequest(ModelState);
            }

            var foundEvent = await _calendarRepository.Find(id, user.UserName);

            if (foundEvent == null)
            {
                return NotFound();
            }

            await _calendarRepository.UpdateEventAsync(updateEvent);

            return new NoContentResult();
        }
    }
}
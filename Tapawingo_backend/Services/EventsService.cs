using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Services
{
    public class EventsService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IMapper _mapper;
        
        public EventsService(IEventsRepository eventsRepository, IMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
        }
        
        public IActionResult GetEventsByOrganisationId(int organisationId)
        {
            var events = _mapper.Map<List<EventDto>>(_eventsRepository.GetEventsByOrganisationId(organisationId));
            return new ObjectResult(events);
        }
        
        public IActionResult getEventByIdAndOrganisationId(int eventId, int organisationId)
        {
            if (!_eventsRepository.EventExists(eventId))
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });
            
            var twEvent = _mapper.Map<EventDto>(_eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId));
            if (twEvent == null)
            {
                return new ForbidResult();
            }
            return new ObjectResult(twEvent);
        }
    }
}
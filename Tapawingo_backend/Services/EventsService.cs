using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Services
{
    public class EventsService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IOrganisationsRepository _organisationsRepository;
        private readonly IOrganisationsRepository _organisationRepository;
        private readonly IMapper _mapper;

        public EventsService(IEventsRepository eventsRepository, IMapper mapper, IOrganisationsRepository organisationsRepository)
        
        public EventsService(IEventsRepository eventsRepository, IMapper mapper, IOrganisationsRepository organisationRepository)
        {
            _eventsRepository = eventsRepository;
            _organisationRepository = organisationRepository;
            _mapper = mapper;
            _organisationsRepository = organisationsRepository;
        }

        public IActionResult GetEventsByOrganisationId(int organisationId)
        {
            //TODO add after OrganisationExists gets merged
            // if (!_organisationRepository.OrganisationExists(organisationId))
            // {
            //     throw new ArgumentException("Organisation does not exist");
            // }
            var events = _mapper.Map<List<EventDto>>(_eventsRepository.GetEventsByOrganisationId(organisationId));
            if (events.IsNullOrEmpty())
            {
                throw new KeyNotFoundException("No events found for this organisation");
            }
            return new ObjectResult(events);
        }
        
        public IActionResult GetEventByIdAndOrganisationId(int eventId, int organisationId)
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

        public Event CreateOrUpdateEvent(CreateEventDto model, int organisationId, int? eventId)
        {
            Event eventEntity;
            if (eventId == null)
            {
                if (!_organisationsRepository.OrganisationExists(organisationId))
                {
                    throw new ArgumentException("Organisation does not exist");
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    throw new ArgumentException("Name is required");
                }

                var eventExists = _eventsRepository.EventExistsForOrganisation(model.Name, organisationId);
                if (eventExists)
                {
                    throw new InvalidOperationException("Event already exists for this organisation");
                }

                eventEntity = _mapper.Map<Event>(model);
                eventEntity.OrganisationId = organisationId;
                return _eventsRepository.CreateEvent(eventEntity);
            }
            else
            {
                eventEntity = _eventsRepository.getEventsByIdAndOrganisationId();
                if (eventEntity == null)
                {
                    throw new ArgumentException("Event does not exist");
                }
                _mapper.Map(model, eventEntity);
                return _eventsRepository.UpdateEvent(eventEntity);
            }
        }
    }
}
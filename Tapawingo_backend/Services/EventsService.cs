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
        private readonly IOrganisationsRepository _organisationsRepository;
        private readonly IMapper _mapper;

        public EventsService(IEventsRepository eventsRepository, IMapper mapper, IOrganisationsRepository organisationsRepository)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
            _organisationsRepository = organisationsRepository;
        }

        public IActionResult GetEventsByOrganisationId(int organisationId)
        {
            var events = _mapper.Map<List<EventDto>>(_eventsRepository.GetEventsByOrganisationId(organisationId));
            return new ObjectResult(events);
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
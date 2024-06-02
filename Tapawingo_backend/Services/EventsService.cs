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

        public Event CreateEvent(CreateEventDto model, int organisationId)
        {
            //TODO Implement when organisation functionality is ready
            // if (!_organisationsRepository.OrganisationExists(organisationId))
            // {
            //     throw new ArgumentException("Organisation does not exist");
            // }
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("Name is required");
            }

            var eventExists = _eventsRepository.EventExistsForOrganisation(model.Name, organisationId);
            if (eventExists)
            {
                throw new InvalidOperationException("Event already exists for this organisation");
            }

            var newEvent = _mapper.Map<Event>(model);
            newEvent.OrganisationId = organisationId;
            return _eventsRepository.CreateEvent(newEvent);
        }
    }
}
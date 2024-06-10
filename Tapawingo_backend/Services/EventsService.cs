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
        private readonly IMapper _mapper;
        public EventsService(IEventsRepository eventsRepository, IMapper mapper, IOrganisationsRepository organisationRepository)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
            _organisationsRepository = organisationRepository;
        }

        public List<EventDto> GetEventsByOrganisationId(int organisationId)
        {
            return _mapper.Map<List<EventDto>>(_eventsRepository.GetEventsByOrganisationId(organisationId));
        }
        
        public EventDto GetEventByIdAndOrganisationId(int eventId, int organisationId)
        {
            return _mapper.Map<EventDto>(_eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId));
            
        }

        public IActionResult CreateEvent(CreateEventDto model, int organisationId)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult("Organisation does not exist");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return new BadRequestObjectResult("Event name is required");
            }

            var eventExists = _eventsRepository.EventExistsForOrganisation(model.Name, organisationId);
            if (eventExists)
            {
                return new ConflictObjectResult("Event already exists for this organisation");
            }
            var eventEntity = _mapper.Map<Event>(model);
            eventEntity.OrganisationId = organisationId;
            _eventsRepository.CreateEvent(eventEntity);
            return new ObjectResult(eventEntity);
        }

        public IActionResult UpdateEvent(CreateEventDto model, int organisationId, int eventId)
        {
            var eventEntity = _eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId);
            if (eventEntity == null)
            {
                return new BadRequestObjectResult("Event does not exist");
            }
    
            if (string.IsNullOrEmpty(model.Name))
            {
                return new BadRequestObjectResult("Event name is required");
            }

            _mapper.Map(model, eventEntity);
            _eventsRepository.UpdateEvent(eventId, eventEntity);
            return new ObjectResult(eventEntity);
        }
        
        public IActionResult DeleteEvent(int eventId, int organisationId)
        {
            var eventEntity = _eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId);
            if (eventEntity == null)
            {
                return new NotFoundObjectResult("Event does not exist");
            }
            _eventsRepository.DeleteEvent(eventEntity.Id);
            return new NoContentResult();
        }
    }
}
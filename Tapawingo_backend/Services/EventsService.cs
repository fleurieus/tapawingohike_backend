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

        public IActionResult GetEventsByOrganisationId(int organisationId)
        {
            if(!_organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            }
            return new OkObjectResult(_mapper.Map<List<EventDto>>(_eventsRepository.GetEventsByOrganisationId(organisationId)));
        }
        
        public IActionResult GetEventByIdAndOrganisationId(int eventId, int organisationId)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            }
            if(!_eventsRepository.EventExists(eventId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });
            }
            if(!_eventsRepository.EventExistsOnOrganisation(organisationId, eventId))
            {
                return new ConflictObjectResult(new
                {
                    message = "Event does not exist on this organisation"
                });
            }
            return new OkObjectResult(_mapper.Map<EventDto>(_eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId)));
        }

        public IActionResult CreateEvent(CreateEventDto model, int organisationId)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation does not exist"
                });
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                return new BadRequestObjectResult(new
                {
                    message = "Event name is required"
                });
            }

            var eventExists = _eventsRepository.EventExistsForOrganisation(model.Name, organisationId);
            if (eventExists)
            {
                return new ConflictObjectResult(new
                {
                    message = "Event already exists for this organisation"
                });
            }
            var eventEntity = _mapper.Map<Event>(model);
            eventEntity.OrganisationId = organisationId;
            _eventsRepository.CreateEvent(eventEntity);
            return new ObjectResult(_mapper.Map<EventDto>(eventEntity));
        }

        public IActionResult UpdateEvent(CreateEventDto model, int organisationId, int eventId)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            }
            if (!_eventsRepository.EventExists(eventId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });
            }
            if (!_eventsRepository.EventExistsOnOrganisation(organisationId, eventId))
            {
                return new ConflictObjectResult(new
                {
                    message = "Event does not exist on this organisation"
                });
            }
            if(model.Name == null || model.Name.Length == 0)
            {
                return new BadRequestObjectResult(new
                {
                    message = "Event name is required"
                });
            }

            var eventEntity = _eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId);

            _mapper.Map(model, eventEntity);
            _eventsRepository.UpdateEvent(eventId, eventEntity);
            return new ObjectResult(_mapper.Map<EventDto>(eventEntity));
        }
        
        public IActionResult DeleteEvent(int eventId, int organisationId)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            }
            if (!_eventsRepository.EventExists(eventId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });
            }
            if (!_eventsRepository.EventExistsOnOrganisation(organisationId, eventId))
            {
                return new ConflictObjectResult(new
                {
                    message = "Event does not exist on this organisation"
                });
            }
            var eventEntity = _eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId);

            _eventsRepository.DeleteEvent(eventEntity.Id);
            return new NoContentResult();
        }
    }
}
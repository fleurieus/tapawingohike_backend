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

        public async Task<IActionResult> GetEventsByOrganisationId(int organisationId)
        {
            if(!await _organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            }
            return new OkObjectResult(_mapper.Map<List<EventDto>>(await _eventsRepository.GetEventsByOrganisationId(organisationId)));
        }
        
        public async Task<IActionResult> GetEventByIdAndOrganisationId(int eventId, int organisationId)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            }
            if(!await _eventsRepository.EventExists(eventId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });
            }
            if(!await _eventsRepository.EventExistsOnOrganisation(organisationId, eventId))
            {
                return new ConflictObjectResult(new
                {
                    message = "Event does not exist on this organisation"
                });
            }
            return new OkObjectResult(_mapper.Map<EventDto>(await _eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId)));
        }

        public async Task<IActionResult> CreateEvent(CreateEventDto model, int organisationId)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
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

            var eventExists = await _eventsRepository.EventExistsForOrganisation(model.Name, organisationId);
            if (eventExists)
            {
                return new ConflictObjectResult(new
                {
                    message = "Event already exists for this organisation"
                });
            }
            var eventEntity = _mapper.Map<Event>(model);
            eventEntity.OrganisationId = organisationId;
            await _eventsRepository.CreateEvent(eventEntity);
            return new ObjectResult(_mapper.Map<EventDto>(eventEntity));
        }

        public async Task<IActionResult> UpdateEvent(CreateEventDto model, int organisationId, int eventId)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            }
            if (!await _eventsRepository.EventExists(eventId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });
            }
            if (!await _eventsRepository.EventExistsOnOrganisation(organisationId, eventId))
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

            var eventEntity = await _eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId);

            _mapper.Map(model, eventEntity);
            await _eventsRepository.UpdateEvent(eventId, eventEntity);
            return new ObjectResult(_mapper.Map<EventDto>(eventEntity));
        }
        
        public async Task<IActionResult> DeleteEvent(int eventId, int organisationId)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            }
            if (!await _eventsRepository.EventExists(eventId))
            {
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });
            }
            if (!await _eventsRepository.EventExistsOnOrganisation(organisationId, eventId))
            {
                return new ConflictObjectResult(new
                {
                    message = "Event does not exist on this organisation"
                });
            }
            var eventEntity = _eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId);

            await _eventsRepository.DeleteEvent(eventEntity.Id);
            return new NoContentResult();
        }
    }
}
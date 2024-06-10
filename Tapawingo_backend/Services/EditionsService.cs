using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Services
{
    public class EditionsService
    {
        private readonly IMapper _mapper;
        private readonly IEditionsRepository _editionsRepository;
        private readonly IOrganisationsRepository _organisationsRepository;
        private readonly IEventsRepository _eventsRepository;

        public EditionsService(IMapper mapper, IEditionsRepository editionRepository, IOrganisationsRepository organisationsRepository, IEventsRepository eventsRepository) {
            _editionsRepository = editionRepository;
            _mapper = mapper;
            _organisationsRepository = organisationsRepository;
            _eventsRepository = eventsRepository;
        }

        public List<EditionDto> GetAllEditions(int eventId)
        {
            if (!_eventsRepository.EventExists(eventId))
            {
                return new List<EditionDto>();
            }
            return _mapper.Map<List<EditionDto>>(_editionsRepository.GetAllEditions(eventId));
        }

        public IActionResult CreateEdition(CreateEditionDto model, int organisationId, int eventId)
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

            if (string.IsNullOrEmpty(model.Name))
            {
                return new BadRequestObjectResult(new 
                { 
                    message = "Name is required"
                });
            }

            var eventExists = _eventsRepository.GetEventByIdAndOrganisationId(eventId, organisationId);

            if (eventExists == null)
            {
                throw new InvalidOperationException("Event does not exists for this organisation");
            }

            var editionEntity = _mapper.Map<Edition>(model);
            editionEntity.EventId = eventId;
            return new ObjectResult(_editionsRepository.CreateEdition(editionEntity));
        }
    }
}

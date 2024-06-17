using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

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

        public EditionDto GetEditionById(int eventId, int editionId)
        {
            try
            {
                if (!_eventsRepository.EventExists(eventId))
                {
                    throw new ArgumentException("Event not found");
                }
                return _mapper.Map<EditionDto>(_editionsRepository.GetEditionById(eventId, editionId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EditionDto> GetAllEditions(int eventId)
        {
            if (!_eventsRepository.EventExists(eventId))
            {
                throw new ArgumentException("Event not found");
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

        public async Task<EditionDto> UpdateEditionAsync(int eventId, int editionId, UpdateEditionDto model)
        {
            if (!_eventsRepository.EventExists(eventId))
                throw new BadHttpRequestException("Event not found");

            if (!_editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            Edition edition = _editionsRepository.GetEditionById(eventId, editionId);

            return _mapper.Map<EditionDto>(await _editionsRepository.UpdateEditionAsync(edition, model));
        }

        public async Task<IActionResult> DeleteEditionAsync(int eventId, int editionId)
        {
            if (!_eventsRepository.EventExists(eventId))
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });

            if (!_editionsRepository.EditionExists(editionId))
                return new NotFoundObjectResult(new
                {
                    message = "Edition not found"
                });

            bool editionDeleted = await _editionsRepository.DeleteEditionAsync(eventId, editionId);
            return editionDeleted ? new NoContentResult() : new BadRequestObjectResult(new
            {
                message = "Edition could not be deleted"
            });
        }
    }
}

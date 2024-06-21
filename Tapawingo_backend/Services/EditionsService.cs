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

        public async Task<EditionDto> GetEditionById(int eventId, int editionId)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");
            if (!await _eventsRepository.EventExists(eventId))
                throw new ArgumentException("Event not found");
            if (!await _editionsRepository.EventExistsOnEdition(eventId, editionId))
                throw new ArgumentException("Event does not exist on edition");

            Edition edition = await _editionsRepository.GetEditionById(editionId);
            if (edition == null)
                throw new BadHttpRequestException("Edition does not exist on event");
            else
                return _mapper.Map<EditionDto>(edition);
        }

        public async Task<List<EditionDto>> GetAllEditions(int eventId)
        {
            if (!await _eventsRepository.EventExists(eventId))
            {
                throw new ArgumentException("Event not found");
            }
            return _mapper.Map<List<EditionDto>>(await _editionsRepository.GetAllEditions(eventId));
        }

        public async Task<EditionDto> CreateEditionOnEventAsync(int eventId, CreateEditionDto createEditionDto)
        {
            if (!await _eventsRepository.EventExists(eventId))
                throw new BadHttpRequestException("Event not found");

            Edition edition = new Edition
            {
                EventId = eventId,
                Name = createEditionDto.Name,
                StartDate = createEditionDto.StartDate,
                EndDate = createEditionDto.EndDate
            };

            return _mapper.Map<EditionDto>(await _editionsRepository.CreateEditionOnEventAsync(edition));
        }

        public async Task<EditionDto> UpdateEditionAsync(int eventId, int editionId, UpdateEditionDto model)
        {
            if (!await _eventsRepository.EventExists(eventId))
                throw new BadHttpRequestException("Event not found");

            if (!await _editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");
            if (!await _editionsRepository.EventExistsOnEdition(eventId, editionId))
                throw new ArgumentException("Event does not exist on edition");

            Edition edition = await _editionsRepository.GetEditionById(editionId);

            return _mapper.Map<EditionDto>(await _editionsRepository.UpdateEditionAsync(edition, model));
        }

        public async Task<IActionResult> DeleteEditionAsync(int eventId, int editionId)
        {
            if (!await _eventsRepository.EventExists(eventId))
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });

            if (!await _editionsRepository.EditionExists(editionId))
                return new NotFoundObjectResult(new
                {
                    message = "Edition not found"
                });
            if (!await _editionsRepository.EventExistsOnEdition(eventId, editionId))
                throw new ArgumentException("Event does not exist on edition");

            bool editionDeleted = await _editionsRepository.DeleteEditionAsync(editionId);
            return editionDeleted ? new NoContentResult() : new BadRequestObjectResult(new
            {
                message = "Edition could not be deleted"
            });
        }
    }
}

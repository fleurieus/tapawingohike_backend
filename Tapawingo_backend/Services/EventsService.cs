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
        private readonly IOrganisationsRepository _organisationRepository;
        private readonly IMapper _mapper;
        
        public EventsService(IEventsRepository eventsRepository, IMapper mapper, IOrganisationsRepository organisationRepository)
        {
            _eventsRepository = eventsRepository;
            _organisationRepository = organisationRepository;
            _mapper = mapper;
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
                throw new ArgumentException("No events found for this organisation");
            }
            return new ObjectResult(events);
        }
    }
}
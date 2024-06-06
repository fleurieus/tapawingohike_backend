using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos; // Importing the Dtos namespace
using Tapawingo_backend.Models; // Importing the Models namespace
using Tapawingo_backend.Repository; // Importing the Repository namespace

namespace Tapawingo_backend.Services
{
    // Service class for handling team-related operations
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository; // Repository for interacting with team data
        private readonly IMapper _mapper;

        // Constructor to initialize the service with the team repository
        public TeamService(ITeamRepository teamRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        // Method to create a new team based on the provided DTO model
        public CreateTeamDto CreateTeam(CreateTeamDto model)
        {
            // Check if input is valid
            if(
                model.Name == null || model.Name.Equals("") || model.Name.Length == 0 ||
                model.Code == null || model.Code.Length == 0 || model.Code.Equals("") ||
                model.ContactName == null || model.ContactName.Equals("") || model.ContactName.Length == 0 ||
                model.ContactEmail == null || model.ContactEmail.Equals("") || model.ContactEmail.Length == 0 ||
                model.ContactPhone == null || model.ContactPhone.Equals("") || model.ContactPhone.Length == 0 ||
                model.Online == null
                )
            {
                throw new ArgumentException("Please make sure all fields are filled correctly.");
            }
            if(model.EditionId == null || model.EditionId == 0) //Add Check if this editionid exists or not.
            {
                throw new InvalidOperationException("EditionId is not valid.");
            }
            // Creating a new Team object using data from the DTO model
            return _mapper.Map<CreateTeamDto>(_teamRepository.CreateTeam(model));
        }
    }
}

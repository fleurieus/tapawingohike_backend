using Tapawingo_backend.Dtos; // Importing the Dtos namespace
using Tapawingo_backend.Models; // Importing the Models namespace
using Tapawingo_backend.Repository; // Importing the Repository namespace

namespace Tapawingo_backend.Services
{
    // Service class for handling team-related operations
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository; // Repository for interacting with team data

        // Constructor to initialize the service with the team repository
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        // Method to create a new team based on the provided DTO model
        public async Task<Team> CreateTeam(CreateTeamDto model)
        {

            /**
            //Check to see that the Edition object for the EditionId passed in the CreateTeamDto exists
            var edition = await _editionService.GetTeam(model.EditionId);
            if (edition == null) {
                return StatusCode(StatusCodes.Status400BadRequest, "This Edition does not exist");
                //Add logging for this maybe.
            }
            **/
            
            // Creating a new Team object using data from the DTO model
            var team = new Team
            {
                Name = model.Name,
                Code = model.Code,
                ContactName = model.ContactName,
                ContactEmail = model.ContactEmail,
                ContactPhone = model.ContactPhone,
                Online = model.Online,
                EditionId = model.EditionId
            };

            // Adding the new team to the repository and returning the result
            return await _teamRepository.AddTeam(team);
        }
    }
}

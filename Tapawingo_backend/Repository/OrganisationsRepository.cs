using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class OrganisationsRepository : IOrganisationsRepository
    {
        private readonly DataContext _context;

        public OrganisationsRepository(DataContext context)
        {
            _context = context;
        }

        public Organisation CreateOrganisation(CreateOrganisationDto createOrganisationDto) 
        {
            var newOrganisation = new Organisation()
            {
                Name = createOrganisationDto.Name,
                ContactPerson = createOrganisationDto.ContactPerson,
                ContactEmail = createOrganisationDto.ContactEmail,
            };
            //add the newly created element to the database without ID because it's auto-created.
            _context.Organisations.Add(newOrganisation);
            //Save changes actually saves it to the database and also it fills the rest of the object. Id is set after executing this.
            _context.SaveChanges();
            return newOrganisation;
        }

        public List<Organisation> GetAllOrganisations()
        {
            return _context.Organisations.ToList();
        }

        public async Task<Organisation> GetOrganisationById(int id)
        {
            var foundOrganisation = await _context.Organisations.FirstOrDefaultAsync(organisation => organisation.Id == id);
            if(foundOrganisation == null)
            {
                return null;
            }
            return foundOrganisation;
        }
        
        public bool OrganisationExists(int id)
        {
            return _context.Organisations.Any(organisation => organisation.Id == id);
        }

        public async Task<Organisation> UpdateOrganisationAsync(int id, UpdateOrganisationDto newOrganisation)
        {
            var oldOrganisation = await GetOrganisationById(id);
            oldOrganisation.Name = newOrganisation.Name == null ? oldOrganisation.Name : newOrganisation.Name;
            oldOrganisation.ContactEmail = newOrganisation.ContactEmail == null ? oldOrganisation.ContactEmail : newOrganisation.ContactEmail;
            oldOrganisation.ContactPerson = newOrganisation.ContactPerson == null ? oldOrganisation.ContactPerson : newOrganisation.ContactPerson;
            
            _context.Organisations.Update(oldOrganisation);
            _context.SaveChanges();
            return oldOrganisation;
        }

        public async Task<bool> DeleteOrganisationAsync(int id)
        {
            var organisation = await GetOrganisationById(id);
            if(organisation == null) {  return false; }
            _context.Organisations.Remove(organisation);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

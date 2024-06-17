using Microsoft.AspNetCore.Identity;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class LocationlogsRepository : ILocationlogsRepository
    {
        private readonly DataContext _context;

        public LocationlogsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Locationlog> CreateLocationlogOnTeamAsync(Locationlog locationlog)
        {
            _context.Locationlogs.Add(locationlog);
            await _context.SaveChangesAsync();

            return locationlog;
        }   
    }
}

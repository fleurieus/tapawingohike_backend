using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class TeamroutepartsRepository : ITeamroutepartsRepository
    {
        private readonly DataContext _context;

        public TeamroutepartsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TeamRoutepart> UpdateTeamRoutePart(int teamId, int routepartId, bool finished)
        {
            //find the teamroutepart
            var teamroutepart = await _context.TeamRouteparts
                .Include(trp => trp.Routepart)
                .FirstOrDefaultAsync(trp => trp.TeamId == teamId && trp.RoutepartId == routepartId);

            if(teamroutepart != null)
            {
                if (teamroutepart.IsFinished && !finished)
                {
                    //already finished, reset completedtime
                    teamroutepart.CompletedTime = DateTime.MinValue;
                }
                else
                {
                    teamroutepart.CompletedTime = DateTime.Now;
                }
                
                teamroutepart.IsFinished = finished;

                _context.TeamRouteparts.Update(teamroutepart);
                await _context.SaveChangesAsync();
                return teamroutepart;
            }
            else
            {
                return null;
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class RoutepartsRepository : IRoutepartsRepository
    {
        private readonly DataContext _context;

        public RoutepartsRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task<List<Routepart>> GetRoutepartsAsync(int route_id)
        {
            return await _context.Routeparts.Where(rp => rp.RouteId == route_id).ToListAsync();
        }

        public async Task<Routepart> GetRoutepartOnRouteAsync(int routeId, int routepartId)
        {
            return await _context.Routeparts
                .Include(rp => rp.Destinations)
                .Include(d => d.Files)
                .FirstOrDefaultAsync(rp => rp.RouteId == routeId && rp.Id == routepartId);
        }

        public async Task<bool> RoutepartExists(int routepartId)
        {
            return await _context.Routeparts.AnyAsync(rp => rp.Id == routepartId);
        }

        public async Task<Routepart> CreateRoutePartAsync(Routepart newRoutepart)
        {
            //set final of all the others to false, set final to this on true
            foreach (var routepart in await GetRoutepartsAsync(newRoutepart.RouteId))
            {
                routepart.Final = false;
            }
            await _context.SaveChangesAsync();

            _context.Routeparts.Add(newRoutepart);
            await _context.SaveChangesAsync();
            return newRoutepart;
        }

        public async Task<Routepart> UpdateRoutepartOnRouteAsync(Routepart existingRoutepart, UpdateRoutepartDto updateRoutepartDto)
        {
            existingRoutepart.Name = updateRoutepartDto.Name;
            existingRoutepart.RouteType = updateRoutepartDto.RouteType;
            existingRoutepart.RoutepartZoom = updateRoutepartDto.RoutepartZoom;
            existingRoutepart.RoutepartFullscreen = updateRoutepartDto.RoutepartFullscreen;
            existingRoutepart.Final = updateRoutepartDto.Final;

            _context.Routeparts.Update(existingRoutepart);
            await _context.SaveChangesAsync();

            if (updateRoutepartDto.Destinations != null)
            {
                // Fetch existing destinations for the route part
                var existingDestinations = await _context.Destinations
                                                         .Where(d => d.RoutepartId == existingRoutepart.Id)
                                                         .ToListAsync();

                // Update or add new destinations
                foreach (var destinationDto in updateRoutepartDto.Destinations)
                {
                    if (destinationDto.Id != null)
                    {
                        var existingDestination = existingDestinations.FirstOrDefault(d => d.Id == destinationDto.Id);
                        if (existingDestination != null)
                        {
                            existingDestination.Name = destinationDto.Name;
                            existingDestination.Latitude = destinationDto.Latitude;
                            existingDestination.Longitude = destinationDto.Longitude;
                            existingDestination.Radius = destinationDto.Radius;
                            existingDestination.DestinationType = destinationDto.DestinationType;
                            existingDestination.ConfirmByUser = destinationDto.ConfirmByUser;
                            existingDestination.HideForUser = destinationDto.HideForUser;
                            _context.Destinations.Update(existingDestination);
                        }
                    }
                    else
                    {
                        var newDestination = new Destination()
                        {
                            RoutepartId = existingRoutepart.Id,
                            Name = destinationDto.Name,
                            Latitude = destinationDto.Latitude,
                            Longitude = destinationDto.Longitude,
                            Radius = destinationDto.Radius,
                            DestinationType = destinationDto.DestinationType,
                            ConfirmByUser = destinationDto.ConfirmByUser,
                            HideForUser = destinationDto.HideForUser
                        };
                        _context.Destinations.Add(newDestination);
                    }
                }

                // Remove destinations that are not in the update request
                foreach (var existingDestination in existingDestinations)
                {
                    if (!updateRoutepartDto.Destinations.Any(d => d.Id == existingDestination.Id))
                    {
                        _context.Destinations.Remove(existingDestination);
                    }
                }
            }
            await _context.SaveChangesAsync();

            if (updateRoutepartDto.Files != null)
            {
                var existingFiles = await _context.Files.Where(f => f.RoutepartId == existingRoutepart.Id).ToListAsync();

                foreach (var fileDto in updateRoutepartDto.Files)
                {
                    var existingFile = existingFiles.FirstOrDefault(f => f.File == fileDto.FileName);
                    if (existingFile != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await fileDto.CopyToAsync(memoryStream);
                            existingFile.ContentType = fileDto.ContentType;
                            existingFile.Data = memoryStream.ToArray();
                            existingFile.UploadDate = DateTime.UtcNow;
                            _context.Files.Update(existingFile);
                        }
                    }
                    else
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await fileDto.CopyToAsync(memoryStream);
                            var newFile = new TWFile
                            {
                                RoutepartId = existingRoutepart.Id,
                                File = fileDto.FileName,
                                ContentType = fileDto.ContentType,
                                Data = memoryStream.ToArray(),
                                UploadDate = DateTime.UtcNow
                            };
                            _context.Files.Add(newFile);
                        }
                    }
                }

                foreach (var existingFile in existingFiles)
                {
                    if (!updateRoutepartDto.Files.Any(f => f.FileName == existingFile.File))
                    {
                        _context.Files.Remove(existingFile);
                    }
                }
            }
            await _context.SaveChangesAsync();

            return existingRoutepart;
        }

        public async Task<bool> DeleteRoutepartOnRouteAsync(int routeId, Routepart routepart)
        {
            try
            {
                //delete the routepart from the database
                _context.Routeparts.Remove(routepart);
                await _context.SaveChangesAsync();

                //get current list of routeparts
                var currentRouteparts = await _context.Routeparts
                    .Where(rp => rp.RouteId == routeId)
                    .OrderBy(rp => rp.Order)
                    .ToListAsync();

                //find out new order
                for (int i = 0; i < currentRouteparts.Count; i++)
                {
                    //change order now
                    currentRouteparts[i].Order = i + 1;

                    //set new final
                    if (i + 1 == currentRouteparts.Count)
                    {
                        currentRouteparts[i].Final = true;
                    }
                    else
                    {
                        currentRouteparts[i].Final = false;
                    }
                }

                _context.Routeparts.UpdateRange(currentRouteparts);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task SyncTeamRoutePartsBasedOnTeam(int editionId, int teamId)
        {
            //check if route parts exist
            var routes = await _context.Routes.Where(r => r.EditionId == editionId).ToListAsync();
            if(routes.Count > 0)
            {
                foreach (var route in routes) 
                {
                    //check if any routeparts exist
                    var routeParts = await _context.Routeparts.Where(rp => rp.RouteId == route.Id).ToListAsync();

                    if(routeParts.Count > 0)
                    {
                        foreach (var routepart in routeParts)
                        {
                            //if route parts exist, each route part to this team in teamrouteparts table

                            //check if the combination exists already

                            _context.TeamRouteparts.Add(new TeamRoutepart
                            {
                                RoutepartId = routepart.Id,
                                TeamId = teamId,
                                IsFinished = false
                            });
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task SyncTeamRoutePartsBasedOnRoutepart(int routeId, int routepartId)
        {
            //collect edition id
            var route = await _context.Routes.FirstOrDefaultAsync(r => r.Id == routeId);
            if(route != null)
            {
                var editionId = route.EditionId;

                //check if any teams are connected to edition
                var connectedTeams = _context.Teams.Where(t => t.EditionId == editionId).ToList();
                
                if(connectedTeams.Count > 0)
                {
                    //if teams are connected to edition, add a row for each team with this routepart
                    var routePart = await _context.Routeparts.FirstOrDefaultAsync(rp => rp.Id == routepartId);

                    foreach (var team in connectedTeams)
                    {
                        //check if the combination exists
                        _context.TeamRouteparts.Add(new TeamRoutepart
                        {
                            RoutepartId = routePart.Id,
                            TeamId = team.Id,
                            IsFinished = false
                        });
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}

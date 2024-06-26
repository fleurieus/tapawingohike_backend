using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository;

public class EventsRepository : IEventsRepository
{
    private readonly DataContext _context;
    
    public EventsRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<Event>> GetEventsByOrganisationId(int organisationId, int? eventId)
    {
        if (eventId != null)
        {
            return await _context.Events.Where(e => e.OrganisationId == organisationId && e.Id == eventId).ToListAsync();
        }
        else
        {
            return await _context.Events.Where(e => e.OrganisationId == organisationId).ToListAsync();
        }
    }
    
    public async Task<Event> GetEventById(int eventId)
    {
        return await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
    }
    
    public async Task<bool> EventExists(int eventId)
    {
        bool eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
        return eventExists;
    }
    public async Task<Event> CreateEvent(Event newEvent)
    {
        
        await _context.Events.AddAsync(newEvent);
        await _context.SaveChangesAsync();
        return newEvent;
    }
    
    public async Task<Event> UpdateEvent(int eventId, Event updatedEvent)
    {
        Event eventToUpdate = await GetEventById(eventId);
        _context.Entry(eventToUpdate).CurrentValues.SetValues(updatedEvent);
        await _context.SaveChangesAsync();
        return updatedEvent;
    }
    
    public async Task<bool> EventExistsForOrganisation(string eventName, int organisationId)
    {
        return await _context.Events.AnyAsync(e => e.Name == eventName && e.OrganisationId == organisationId);
    }
 
    public async Task<Event> GetEventByIdAndOrganisationId(int eventId, int organisationId)
    {
        return await _context.Events.Where(e => e.Id == eventId && e.OrganisationId == organisationId).FirstOrDefaultAsync();
    }
    
    public async Task DeleteEvent(int eventId)
    {
        Event eventToDelete = await GetEventById(eventId);
        
        _context.Events.Remove(eventToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EventExistsOnOrganisation(int organisationId, int eventId)
    {
        var foundEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        if (foundEvent == null) return false;
        return foundEvent.OrganisationId == organisationId;
    }
}
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
    
    public ICollection<Event> GetEventsByOrganisationId(int organisationId)
    {
        return _context.Events.Where(e => e.OrganisationId == organisationId).ToList();
    }
    
    public Event CreateEvent(Event newEvent)
    {
        _context.Events.Add(newEvent);
        _context.SaveChanges();
        return newEvent;
    }
    
    public Event UpdateEvent(Event updatedEvent)
    {
        _context.Events.Update(updatedEvent);
        _context.SaveChanges();
        return updatedEvent;
    }
    
    public bool EventExistsForOrganisation(string eventName, int organisationId)
    {
        return _context.Events.Any(e => e.Name == eventName && e.OrganisationId == organisationId);
    }
 
}
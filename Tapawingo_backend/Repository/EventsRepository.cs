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
    
    public bool EventExists(int eventId)
    {
        bool eventExists = _context.Events.Any(e => e.Id == eventId);
        return eventExists;
    }
    
    public Event GetEventByIdAndOrganisationId(int eventId, int organisationId)
    {
        return _context.Events.Where(e => e.Id == eventId && e.OrganisationId == organisationId).FirstOrDefault();
    }
}
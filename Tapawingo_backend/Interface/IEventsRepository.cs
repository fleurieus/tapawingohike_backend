using System.Collections;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IEventsRepository
    {
        Task<List<Event>> GetEventsByOrganisationId(int organisationId, int? eventId);
        Task<Event> CreateEvent(Event newEvent);
        Task<Event> UpdateEvent(int eventId, Event updatedEvent);
        Task<bool> EventExistsForOrganisation(string eventName, int organisationId);
        Task<bool> EventExists(int eventId);
        Task<Event> GetEventByIdAndOrganisationId(int eventId, int organisationId);
        Task DeleteEvent(int eventId);
        Task<bool> EventExistsOnOrganisation(int organisationId, int eventId);
    }
}
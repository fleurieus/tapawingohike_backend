using System.Collections;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IEventsRepository
    {
        ICollection<Event> GetEventsByOrganisationId(int organisationId);
        Event CreateEvent(Event newEvent);
        Event UpdateEvent(Event updatedEvent);
        bool EventExistsForOrganisation(string eventName, int organisationId);
        bool EventExists(int eventId);
        Event GetEventByIdAndOrganisationId(int eventId, int organisationId);
    }
}
using System.Collections;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IEventsRepository
    {
        List<Event> GetEventsByOrganisationId(int organisationId);
        Event CreateEvent(Event newEvent);
        Event UpdateEvent(int eventId, Event updatedEvent);
        bool EventExistsForOrganisation(string eventName, int organisationId);
        bool EventExists(int eventId);
        Event GetEventByIdAndOrganisationId(int eventId, int organisationId);
        void DeleteEvent(int eventId);
        bool EventExistsOnOrganisation(int organisationId, int eventId);
    }
}
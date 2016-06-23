using SupportApp.Models;

namespace SupportApp.Services
{
    public interface IEventLocationService
    {
        Location GetLocation(int locationId);
        Event SaveAddedLocation(int eventId, string userId, Location location);
        Event SaveEditedUserEvent(string userId, bool hasClaim, int eventId, Event eventData);
        Location SaveLocation(int locationId, string userId, Location location);
    }
}
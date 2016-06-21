using SupportApp.Models;

namespace SupportApp.Services
{
    public interface IEventLocationService
    {
        Event convertDatesFromUtcToLocalTime(Event sglEvent);
        Event DisplayUserFirstName(Event sglEvent);
        Event SaveEventLocation(int eventId, int locationId, Location location);
    }
}
using System.Collections.Generic;
using SupportApp.Models;
using SupportApp.ViewModels.Connection;

namespace SupportApp.Services
{
    public interface IEventService
    {
        EventsViewModel GetAllEvents(bool hasClaim, string id);
        EventsViewModel GetAllHistoryEvents(bool hasClaim, string id);
        EventsViewModel GetUserEvents(string id, bool hasClaim);
        EventsViewModel GetUserHistoryEvents(string id, bool hasClaim);
        EventsViewModel GetActiveEvents();
        EventsViewModel GetHistoryEvents();

        List<Event> GetActiveEventsByCity(string city);
        List<Event> GetActiveHistoryEventsByCity(string city);
        List<Event> GetMyCurrentEventsByCity(string userId, bool hasClaim, string city);
        List<Event> GetMyHistoryEventsByCity(string userId, bool hasClaim, string city);
        List<Event> GetAdminEventsByCity(string userId, bool hasClaim, string city);
        List<Event> GetAdminHistoryEventsByCity(string userId, bool hasClaim, string city);

        EventDetailsViewModel GetEventDetails(string userId, int id);
        EventDetailsViewModel GetUserEventDetails(string userId, bool hasClaim, int id);

        void SaveEvent(string userId, bool hasClaim, EventLocationViewModel addedEvent);
        Event UpdateVotes(string userId, int id, int voteType);
        void DeleteEvent(string userId, bool hasClaim, int id);

    }
}
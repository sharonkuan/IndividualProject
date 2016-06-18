using System.Collections.Generic;
using SupportApp.Models;
using SupportApp.ViewModels.Connection;

namespace SupportApp.Services
{
    public interface IEventService
    {
        EventsViewModel GetAllEvents(bool hasClaim, string id);
        EventsViewModel GetUserEvents(string id);
        EventsViewModel GetActiveEvents();
        EventsViewModel GetHistoryEvents();

        List<Event> GetActiveEventsByCity(string city);
        EventDetailsViewModel GetEventDetails(string userId, int id);
        EventDetailsViewModel GetUserEventDetails(string userId, bool hasClaim, int id);

        Event SaveEvent(string userId, Event addedEvent);
        Event UpdateVotes(string userId, int id, int voteType);
        void DeleteEvent(string userId, bool hasClaim, int id);
    }
}
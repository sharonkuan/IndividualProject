using System.Collections.Generic;
using SupportApp.Models;
using SupportApp.ViewModels.Connection;

namespace SupportApp.Services
{
    public interface IEventService
    {
        void DeleteEvent(int id);
        EventsViewModel GetAllEvents(bool hasClaim, string id);
        EventsViewModel GetUserEvents(string id);
        EventsViewModel GetActiveEvents();
        EventsViewModel GetHistoryEvents();

        List<Event> GetEventByCity(string city);
        //EventDetailsViewModel GetActiveEventDetails(int id);
        EventDetailsViewModel GetActiveEventDetails(string userId, int id);
        EventDetailsViewModel GetHistoryEventDetails(string userId, int id);
        EventDetailsViewModel GetUserEventDetails(string userId, bool hasClaim, int id);

        Event SaveEvent(Event addedEvent);
        Event UpdateVotes(int id, int voteType);
    }
}
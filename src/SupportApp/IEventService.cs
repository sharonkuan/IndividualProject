using System.Collections.Generic;
using SupportApp.Models;
using SupportApp.ViewModels.Event;

namespace SupportApp.Services
{
    public interface IEventService
    {
        void DeleteEvent(int id);
        EventsViewModel GetEvent(int id);
        List<EventsViewModel> GetEvents();
        Event SaveEvent(Event addedEvent);
        Event UpdateVotes(int id, int voteType);
    }
}
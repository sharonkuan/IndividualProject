using SupportApp.Models;
using SupportApp.ViewModels.Connection;

namespace SupportApp.Services
{
    public interface IEventCommentsService
    {
        Event SaveEventComment(int id, string userId, Comment comment);
        void SignUpToVolunteer(string userId, int id, bool adminApproved);
        //EventDetailsViewModel ReloadEventDetailsPage(string userId, int id);
    }
}
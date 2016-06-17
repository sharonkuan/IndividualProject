using SupportApp.Models;
using SupportApp.ViewModels.Connection;

namespace SupportApp.Services
{
    public interface IEventCommentsService
    {
        EventCommentViewModel SaveEventComment(int id, string userId, Comment comment);
    }
}
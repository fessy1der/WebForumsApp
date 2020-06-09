using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebForumsApp.Data.Interfaces
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();
        IEnumerable<ApplicationUser> GetActiveUsers();
        Task Create(Forum forum);
        Task Delete(int forumId);
        Task UpdateTitle(int id, string newTitle);
        Task UpdateDescription(int id, string newDescription);
    }
}

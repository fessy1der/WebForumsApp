using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebForumsApp.Data;
using WebForumsApp.Data.Interfaces;

namespace WebForumsApp.Service
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _db;

        public ForumService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task Create(Forum forum)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int forumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetActiveUsers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Forum> GetAll()
        {
            return _db.Forums.Include(i => i.Posts);
        }

        public Forum GetById(int id)
        {
            var forum = _db.Forums.Where(i => i.Id == id)
                .Include(f => f.Posts).ThenInclude(p => p.User)
                .Include(f => f.Posts).ThenInclude(p => p.Replies).ThenInclude(p => p.User)
                .Include(f => f.Posts).ThenInclude(p => p.Forum).FirstOrDefault();
            return forum;
        }

        public Task UpdateDescription(int id, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTitle(int id, string newTitle)
        {
            throw new NotImplementedException();
        }
    }
}

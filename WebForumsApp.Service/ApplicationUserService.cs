using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebForumsApp.Data;
using WebForumsApp.Data.Interfaces;

namespace WebForumsApp.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(ApplicationUser user)
        {
            _db.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task BumpRating(string userId, Type type)
        {
            var user = GetById(userId);
            var increment = GetIncrement(type);
            user.Rating += increment;
            await _db.SaveChangesAsync();
        }

        private static int GetIncrement(Type type)
        {
            var bump = 0;

            if (type == typeof(Post))
            {
                bump = 3;
            }

            if (type == typeof(Reply))
            {
                bump = 2;
            }

            return bump;
        }

        public async Task Deactivate(ApplicationUser user)
        {
            user.IsActive = false;
            _db.Update(user);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _db.ApplicationUsers;
        }

        public ApplicationUser GetById(string id)
        {
            return _db.ApplicationUsers.FirstOrDefault(user => user.Id == id);
        }

        public async Task IncrementRating(string id)
        {
            var user = GetById(id);
            user.Rating += 1;
            _db.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.UserImage = uri.AbsoluteUri;
            _db.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}

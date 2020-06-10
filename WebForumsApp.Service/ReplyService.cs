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
    public class ReplyService : IReply
    {
        private readonly ApplicationDbContext _db;

        public ReplyService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Delete(int id)
        {
            var reply = GetById(id);
            _db.Remove(reply);
            await _db.SaveChangesAsync();
        }

        public async Task Edit(int id, string message)
        {
            var reply = GetById(id);
            await _db.SaveChangesAsync();
            _db.Update(reply);
            await _db.SaveChangesAsync();
        }

        public Reply GetById(int id)
        {
            return _db.Replies
                .Include(r => r.Post)
                .ThenInclude(post => post.Forum)
                .Include(r => r.Post)
                .ThenInclude(post => post.User).First(r => r.Id == id);
        }
    }
}

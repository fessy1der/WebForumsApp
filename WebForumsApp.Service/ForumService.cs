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
        private readonly IPost _postService;

        public ForumService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(Forum forum)
        {
            _db.Add(forum);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int forumId)
        {
            var forum = GetById(forumId);
            _db.Remove(forum);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<ApplicationUser> GetActiveUsers(int forumId)
        {
            var posts = GetById(forumId).Posts;

            if (posts == null || !posts.Any())
            {
                return new List<ApplicationUser>();
            }

            return _postService.GetAllUsers(posts);
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

        public async Task UpdateDescription(int id, string newDescription)
        {
            var forum = GetById(id);
            forum.Description = newDescription;

            _db.Update(forum);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateTitle(int id, string newTitle)
        {
            var forum = GetById(id);
            forum.Title = newTitle;

            _db.Update(forum);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            return _postService.GetFilteredPosts(searchQuery);
        }

        public IEnumerable<Post> GetFilteredPosts(int forumId, string searchQuery)
        {
            if (forumId == 0) return _postService.GetFilteredPosts(searchQuery);

            var forum = GetById(forumId);

            return string.IsNullOrEmpty(searchQuery)
                ? forum.Posts
                : forum.Posts.Where(post
                    => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
        }

        public async Task SetForumImage(int id, Uri uri)
        {
            var forum = GetById(id);
            forum.Image = uri.AbsoluteUri;
            _db.Update(forum);
            await _db.SaveChangesAsync();
        }

        public Post GetLatestPost(int forumId)
        {
            var posts = GetById(forumId).Posts;

            if (posts != null)
            {
                return GetById(forumId).Posts
                    .OrderByDescending(post => post.DateCreated)
                    .FirstOrDefault();
            }

            return new Post();
        }

        public bool HasRecentPost(int id)
        {
            const int hoursAgo = 12;
            var window = DateTime.Now.AddHours(-hoursAgo);
            return GetById(id).Posts.Any(post => post.DateCreated >= window);
        }


    }
}

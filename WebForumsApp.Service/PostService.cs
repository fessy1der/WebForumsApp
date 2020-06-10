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
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _db;

        public PostService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(Post post)
        {
            _db.Add(post);
            await _db.SaveChangesAsync();
        }

        public async Task AddReply(Reply reply)
        {
            _db.Replies.Add(reply);
            await _db.SaveChangesAsync();
        }

        public Task Archive(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            var post = GetById(id);
            _db.Remove(post);
            await _db.SaveChangesAsync();
        }

        public async Task EditPostContent(int id, string content)
        {
            var post = GetById(id);
            post.Content = content;
            _db.Posts.Update(post);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<Post> GetAll()
        {
            var posts = _db.Posts
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .ThenInclude(reply => reply.User);
            return posts;
        }

        public IEnumerable<ApplicationUser> GetAllUsers(IEnumerable<Post> posts)
        {
            var users = new List<ApplicationUser>();

            foreach (var post in posts)
            {
                users.Add(post.User);

                if (!post.Replies.Any()) continue;

                users.AddRange(post.Replies.Select(reply => reply.User));
            }

            return users.Distinct();
        }

        public Post GetById(int id)
        {
            return _db.Posts.Where(post => post.Id == id)
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .ThenInclude(reply => reply.User)
                .FirstOrDefault();
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            var query = searchQuery.ToLower();

            return _db.Posts
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .Where(post =>
                    post.Title.ToLower().Contains(query)
                 || post.Content.ToLower().Contains(query));
        }

        public string GetForumImageUrl(int id)
        {
            var post = GetById(id);
            return post.Forum.Image;
        }

        public IEnumerable<Post> GetLatestPosts(int count)
        {
            var allPosts = GetAll().OrderByDescending(post => post.DateCreated);
            return allPosts.Take(count);
        }

        public IEnumerable<Post> GetPostsBetween(DateTime start, DateTime end)
        {
            return _db.Posts.Where(post => post.DateCreated >= start && post.DateCreated <= end);
        }

        public IEnumerable<Post> GetPostsByForumId(int id)
        {
            return _db.Forums.First(forum => forum.Id == id).Posts;
        }

        public IEnumerable<Post> GetPostsByUserId(int id)
        {
            return _db.Posts.Where(post => post.User.Id == id.ToString());
        }

        public int GetReplyCount(int id)
        {
            return GetById(id).Replies.Count();
        }
    }
}

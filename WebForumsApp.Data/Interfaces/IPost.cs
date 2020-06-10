using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebForumsApp.Data.Interfaces
{
    public interface IPost
    {
        Task Add(Post post);
        Task Archive(int id);
        Task Delete(int id);
        Task EditPostContent(int id, string content);

        Task AddReply(Reply reply);

        int GetReplyCount(int id);

        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetPostsByUserId(int id);
        IEnumerable<Post> GetPostsByForumId(int id);
        IEnumerable<Post> GetPostsBetween(DateTime start, DateTime end);
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<ApplicationUser> GetAllUsers(IEnumerable<Post> posts);
        IEnumerable<Post> GetLatestPosts(int count);
        string GetForumImageUrl(int id);
    }
}

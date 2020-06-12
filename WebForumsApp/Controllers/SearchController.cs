using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebForumsApp.Data;
using WebForumsApp.Data.Interfaces;
using WebForumsApp.Data.Models.ViewModels;

namespace WebForumsApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPost _postService;

        public SearchController(IPost postService)
        {
            _postService = postService;
        }

        public IActionResult Results(string searchQuery)
        {
            var posts = _postService.GetFilteredPosts(searchQuery).ToList();
            var noResults = (!string.IsNullOrEmpty(searchQuery) && !posts.Any());

            var postListings = posts.Select(post => new PostListingVM
            {
                Id = post.Id,
                Forum = BuildForumListing(post),
                Author = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.DateCreated.ToString(CultureInfo.InvariantCulture),
                NumberOfReplies = post.Replies.Count()
            }).OrderByDescending(post => post.DatePosted);

            var model = new SearchResultVM
            {
                EmptySearchResults = noResults,
                Posts = postListings,
                SearchQuery = searchQuery,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery });
        }

        private static ForumListingVM BuildForumListing(Forum forum)
        {
            return new ForumListingVM
            {
                Id = forum.Id,
                Image= forum.Image,
                Title = forum.Title,
                Description = forum.Description
            };
        }

        private static ForumListingVM BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }
    }
}

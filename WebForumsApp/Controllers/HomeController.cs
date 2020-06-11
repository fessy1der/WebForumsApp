using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebForumsApp.Data.Interfaces;
using WebForumsApp.Data.Models.ViewModels;
using WebForumsApp.Models;

namespace WebForumsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPost _postService;

        public HomeController(ILogger<HomeController> logger, IPost postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexVM();
            return View(model);
        }

        public HomeIndexVM BuildHomeIndexVM()
        {
            var latest = _postService.GetLatestPosts(10);

            var posts = latest.Select(post => new PostListingVM
            {
                Id = post.Id,
                Title = post.Title,
                Author = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.DateCreated.ToString(),
                NumberOfReplies = _postService.GetReplyCount(post.Id),
                ForumName = post.Forum.Title,
                ForumImageUrl = _postService.GetForumImageUrl(post.Id),
                ForumId = post.Forum.Id
            });

            return new HomeIndexVM()
            {
                LatestPosts = posts
            };

        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Topic", "Forum", new { searchQuery });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

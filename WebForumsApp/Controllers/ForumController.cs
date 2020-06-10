using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebForumsApp.Data.Interfaces;
using WebForumsApp.Data.Models.ViewModels;

namespace WebForumsApp.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ForumController(IForum forumService, IConfiguration configuration, IApplicationUser userService, IUpload uploadService)
        {
            _forumService = forumService;
            _configuration = configuration;
            _userService = userService;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll().Select(f => new ForumListingVM
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description,
                NumberOfPosts = f.Posts?.Count() ?? 0,
                Latest = GetLatestPost(f.Id) ?? new PostListingVM(),
                NumberOfUsers = _forumService.GetActiveUsers(f.Id).Count(),
                Image = f.Image,
                HasRecentPost = _forumService.HasRecentPost(f.Id)
            });

            var forumsListingModels = forums as IList<ForumListingVM> ?? forums.ToList();

            var model = new ForumIndexVM
            {
                ForumsList = forumsListingModels.OrderBy(forum => forum.Title),
                NumberOfForums = forumsListingModels.Count()
            };

            return View(model);
        }

        public PostListingVM GetLatestPost(int forumId)
        {
            var post = _forumService.GetLatestPost(forumId);

            if (post != null)
            {
                return new PostListingVM
                {
                    Author = post.User != null ? post.User.UserName : "",
                    DatePosted = post.DateCreated.ToString(CultureInfo.InvariantCulture),
                    Title = post.Title ?? ""
                };
            }

            return new PostListingVM();
        }
    }
}

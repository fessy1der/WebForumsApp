using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using WebForumsApp.Data;
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

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);
            var posts = _forumService.GetFilteredPosts(id, searchQuery).ToList();
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

            var model = new TopicResultVM
            {
                EmptySearchResults = noResults,
                Posts = postListings,
                SearchQuery = searchQuery,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        private static ForumListingVM BuildForumListing(Forum forum)
        {
            return new ForumListingVM
            {
                Id = forum.Id,
                Image = forum.Image,
                Title = forum.Title,
                Description = forum.Description
            };
        }

        private static ForumListingVM BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }

        public IEnumerable<ApplicationUserVM> GetActiveUsers(int forumId)
        {
            return _forumService.GetActiveUsers(forumId).Select(appUser => new ApplicationUserVM
            {
                Id = Convert.ToInt32(appUser.Id),
                UserImage = appUser.UserImage,
                Rating = appUser.Rating,
                Username = appUser.UserName
            });
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        public IActionResult Create()
        {
            var model = new AddForumVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddForum(AddForumVM model)
        {

            var imageUri = "";

            if (model.ImageUpload != null)
            {
                var blockBlob = PostForumImage(model.ImageUpload);
                imageUri = blockBlob.Uri.AbsoluteUri;
            }

            else
            {
                imageUri = "/images/users/default.png";
            }

            var forum = new Forum()
            {
                Title = model.Title,
                Description = model.Description,
                DateCreated = DateTime.Now,
                Image = imageUri
            };

            await _forumService.Create(forum);
            return RedirectToAction("Index", "Forum");
        }

        public CloudBlockBlob PostForumImage(IFormFile file)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccountConnectionString");
            var container = _uploadService.GetBlobContainer(connectionString);
            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.ToString().Trim('"'));
            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return blockBlob;
        }
    }
}

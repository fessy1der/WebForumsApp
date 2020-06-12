using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebForumsApp.Data;
using WebForumsApp.Data.Interfaces;
using WebForumsApp.Data.Models.ViewModels;
using WebForumsApp.Utility;

namespace WebForumsApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;
        private readonly IPostFormater _postFormatter;
        private readonly IApplicationUser _userService;

        private static UserManager<ApplicationUser> _userManager;

        public PostController(IPost postService, IForum forumService, IApplicationUser userService, UserManager<ApplicationUser> userManager, IPostFormater postFormatter)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _postFormatter = postFormatter;
            _userService = userService;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = GetPostReplies(post).OrderBy(reply => reply.Date);

            var model = new PostIndexVM
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.UserImage,
                AuthorRating = post.User.Rating,
                IsAuthorAdmin = IsAuthorAdmin(post.User),
                Date = post.DateCreated,
                PostContent = _postFormatter.Beautify(post.Content),
                Replies = (IEnumerable<ReplyVM>)replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title
            };

            return View(model);
        }

        private IEnumerable<ReplyVM> GetPostReplies(Post post)
        {


            return post.Replies.Select(reply => new ReplyVM
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.UserImage,
                AuthorRating = reply.User.Rating,
                Date = reply.DateCreated,
                ReplyContent = _postFormatter.Beautify(reply.Message),
                IsAuthorAdmin = IsAuthorAdmin(reply.User)
            });
        }

        public static bool IsAuthorAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user)
                .Result.Contains("Admin");
        }

        public IActionResult Create(int id)
        {
            // note id here is Forum.Id
            var forum = _forumService.GetById(id);

            var model = new NewPostVM
            {
                ForumName = forum.Title,
                ForumId = forum.Id,
                AuthorName = User.Identity.Name,
                ForumImageUrl = forum.Image
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostVM model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var post = BuildPost(model, user);

            await _postService.Add(post);
            await _userService.BumpRating(userId, typeof(Post));

            return RedirectToAction("Index", "Forum", model.ForumId);
        }

        public Post BuildPost(NewPostVM post, ApplicationUser user)
        {
            var now = DateTime.Now;
            var forum = _forumService.GetById(post.ForumId);

            return new Post
            {
                Title = post.Title,
                Content = post.Content,
                DateCreated = now,
                Forum = forum,
                User = user,
                IsArchived = false
            };
        }

        public IActionResult Edit(int postId)
        {
            var post = _postService.GetById(postId);

            var model = new NewPostVM
            {
                Title = post.Title,
                Content = post.Content,
                DateCreated = post.DateCreated,
            };

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var post = _postService.GetById(id);
            var model = new DeletePostVM
            {
                PostId = post.Id,
                PostAuthor = post.User.UserName,
                PostContent = post.Content
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var post = _postService.GetById(id);
            _postService.Delete(id);

            return RedirectToAction("Index", "Forum", new { id = post.Forum.Id });
        }
    }
}

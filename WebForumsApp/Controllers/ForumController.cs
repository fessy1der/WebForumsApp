using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebForumsApp.Data;
using WebForumsApp.Data.Interfaces;
using WebForumsApp.Data.Models.ViewModels;

namespace WebForumsApp.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;

        public ForumController(IForum forumService)
        {
            _forumService = forumService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll().Select(f => new ForumListingVM
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description
            });

            var model = new ForumIndexVM
            {
                ForumsList = forums
            };
            return View(model);
        }
    }
}

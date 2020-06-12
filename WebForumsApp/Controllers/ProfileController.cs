using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebForumsApp.Data;
using WebForumsApp.Data.Interfaces;
using WebForumsApp.Data.Models.ViewModels;

namespace WebForumsApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        [Authorize]
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var model = new ProfileVM()
            {
                UserId = user.Id,
                Username = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                UserImage = user.UserImage,
                JoinedOn = user.JoinedOn,
                IsActive = user.IsActive,
                IsAdmin = userRoles.Contains("Admin")
            };

            return View(model);
        }

        /*
         * Files uploaded using the IFormFile technique are buffered in memory or on disk on the web server 
         * before being processed. Inside the action method, the IFormFile contents are accessible as a stream. 
         * In addition to the local file system, files can be streamed to Azure Blob storage or Entity Framework.
         */

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            var connectionString = _configuration.GetConnectionString("AzureStorageAccountConnectionString");
            var container = _uploadService.GetBlobContainer(connectionString);

            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.Trim('"'));

            var blockBlob = container.GetBlockBlobReference(filename);

            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            await _userService.SetProfileImage(userId, blockBlob.Uri);

            return RedirectToAction("Detail", "Profile", new { id = userId });
        }

        public IActionResult Index()
        {
            var profiles = _userService.GetAll()
                .OrderByDescending(user => user.Rating)
                .Select(u => new ProfileVM
                {
                    Email = u.Email,
                    UserImage = u.UserImage,
                    UserRating = u.Rating.ToString(),
                    JoinedOn = u.JoinedOn,
                    IsActive = u.IsActive
                });

            var model = new ProfileListingVM
            {
                Profiles = profiles
            };

            return View(model);
        }

        public IActionResult Deactivate(string userId)
        {
            var user = _userService.GetById(userId);
            _userService.Deactivate(user);
            return RedirectToAction("Index", "Profile");
        }
    }
}

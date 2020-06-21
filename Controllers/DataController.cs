using System.IO;
using DataRoom.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataRoom.Models;

namespace DataRoom.Controllers
{
    public class DataController : Controller
    {
        private readonly UserService userService;
        private readonly FileDataService fileDataService;
        public DataController(UserService uService, FileDataService fDService)
        {
            userService = uService;
            fileDataService = fDService;
        }

        public IActionResult New(string path)
        {
            UserModel user = userService.GetUserModel(HttpContext.Session.GetString("Username"));

            if(user.HasRole(UserRole.DataManager))
            {
                ViewData["CurrentPath"] = path;
                return View();
            }

            return Unauthorized();
        }

        public IActionResult Post(string path)
        {
            string newPath = fileDataService.CreateNewSubjectFolder(path, Request.Form["FolderName"]);
            return RedirectToAction("Index", "Home", new { itemPath = newPath });
        }
    }
}

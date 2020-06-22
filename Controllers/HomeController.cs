using System.Diagnostics;
using System.Linq;
using DataRoom.Helpers;
using DataRoom.Models;
using DataRoom.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;

namespace DataRoom.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserService userService;
        private readonly FileDataService fileDataService;
        public HomeController(UserService uService, FileDataService fDService)
        {
            userService = uService;
            fileDataService = fDService;
        }
        
        [Route("/")]
        [Route("/{itemPath}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(string itemPath = null)
        {

            if (!HttpContext.Session.HasUsername())
            {
                return Redirect("/Login");
            }

            var user = userService.GetUserModel(HttpContext.Session.GetString("Username"));

            // Check user permissions and path validity
            if (!string.IsNullOrEmpty(itemPath) && !fileDataService.IsPathTopLevel(itemPath))
            {
                if (!fileDataService.PathExists(itemPath)) { return NotFound(); }
                if (!user.HasRole(UserRole.DataManager))
                {
                    if (!user.HasPermission(fileDataService.GetSubjectFolderName(itemPath))) { return Unauthorized(); }
                }
            }

            if(!string.IsNullOrEmpty(itemPath))
            {
                if (fileDataService.IsPathFolder(itemPath)) { ViewData["CurrentPath"] = itemPath; }
                else if (fileDataService.IsPathFile(itemPath)) { return GetFileDownload(itemPath); }
            } else
            {
                ViewData["CurrentPath"] = fileDataService.TopLevelDataFolder;
            }

            var fileFolderModel = fileDataService.GetDataForPath(itemPath);
            if (fileFolderModel.FileObjects.Any())
            {
                // Only check permissions if the path is top level or user role is lower than DataManager
                if (fileDataService.IsPathTopLevel(fileFolderModel.FileObjects.First().Parent) && !user.HasRole(UserRole.DataManager))
                {
                    fileFolderModel.FileObjects = fileFolderModel.FileObjects
                        .Where(x => user.HasPermission(fileDataService.GetSubjectFolderName(x.ItemPath)))
                        .ToList();
                }
            }
            return View(fileFolderModel);
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

        [NonAction]
        private IActionResult GetFileDownload(string path)
        {
            var cd = new ContentDispositionHeaderValue("attachment")
            {
                FileName = path.Split("\\")[^1]
            };
            Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());
            var fileData = fileDataService.GetDataForFile(path);
            new FileExtensionContentTypeProvider().TryGetContentType(fileDataService.GetFullPath(path), out string contentType);
            return File(fileData, contentType);
        }


    }
}

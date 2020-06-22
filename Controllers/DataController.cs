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
                if(fileDataService.PathExists(path))
                {
                    ViewData["CurrentPath"] = path;
                    return View();
                } else
                {
                    return NotFound();
                }

            }

            return Unauthorized();
        }

        public IActionResult Rename(string path)
        {

            UserModel user = userService.GetUserModel(HttpContext.Session.GetString("Username"));

            if (user.HasRole(UserRole.DataManager))
            {
                if (fileDataService.PathExists(path))
                {
                    if (fileDataService.IsPathFile(path))
                    {
                        ViewData["ObjectType"] = "Document";
                    }
                    else if (fileDataService.IsPathFolder(path))
                    {
                        ViewData["ObjectType"] = "Folder";
                    }
                    ViewData["CurrentPath"] = path;
                    return View();
                }
                else
                {
                    return NotFound();
                }
            }

            return Unauthorized();
        }

        public IActionResult Delete(string path)
        {

            UserModel user = userService.GetUserModel(HttpContext.Session.GetString("Username"));

            if (user.HasRole(UserRole.DataManager))
            {
                if (fileDataService.PathExists(path))
                {
                    if (fileDataService.IsPathFile(path))
                    {
                        ViewData["ObjectType"] = "Document";
                    }
                    else if (fileDataService.IsPathFolder(path))
                    {
                        ViewData["ObjectType"] = "Folder";
                    }
                    ViewData["CurrentPath"] = path;
                    return View();
                } else
                {
                    return NotFound();
                }
            }

            return Unauthorized();
        }

        public IActionResult ConfirmDelete(string path)
        {

            UserModel user = userService.GetUserModel(HttpContext.Session.GetString("Username"));

            if (user.HasRole(UserRole.DataManager))
            {
                if (fileDataService.PathExists(path))
                {
                    string retUrl = fileDataService.GetRelativeSubjectPath(fileDataService.GetParentPathForFileOrFolder(path));
                    fileDataService.DeleteFileOrFolderForPath(path);
                    return RedirectToAction("Index", "Home", new { itemPath = retUrl });
                }
                else
                {
                    return NotFound();
                }
            }

            return Unauthorized();
        }

        public IActionResult Post(string path)
        {
            string newPath = fileDataService.CreateNewFolder(path, Request.Form["FolderName"]);
            return RedirectToAction("Index", "Home", new { itemPath = newPath });
        }
    }
}

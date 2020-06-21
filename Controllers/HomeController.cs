using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataRoom.Models;
using System.IO;
using Microsoft.AspNetCore.Session;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;

namespace DataRoom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Route("/")]
        [Route("Home/Index")]
        [Route("Home/Index/{path}/")]
        [Route("Home/Index/{path}/{type}")]
        public IActionResult Index(string path = "DataSource", string type = "Directory")
        {
            if(HttpContext.Session.GetString("UserName") != null)
            {
                if (type == "Directory")
                {

                    ViewData["Directories"] = FileSystemModel.GetListDataForPath(path);
                    ViewData["CurrentPath"] = path.Replace("DataSource", "");
                    if (string.IsNullOrEmpty(ViewData["CurrentPath"] as string))
                    {
                        ViewData["CurrentPath"] = "Developments";
                    }
                }
                else if (type == "File")
                {
                    var cd = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = path.Split("\\")[^1]
                    };
                    Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());
                    var fileData = System.IO.File.ReadAllBytes(path);
                    new FileExtensionContentTypeProvider().TryGetContentType(path, out string contentType);
                    return File(fileData, contentType);
                }
                return View();
            } else
            {
                return Redirect("/Auth/Login");
            }
            
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

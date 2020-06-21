using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataRoom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataRoom.Controllers
{
    public class AuthController : Controller
    {
        private Dictionary<string, UserModel> userLogins = new Dictionary<string, UserModel>() { { "lewis.barnes", new UserModel { EmailAddress = "lewis.barnes", Password = "test" }}};
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

        public IActionResult Post(UserModel userModel)
        {
            if(!string.IsNullOrEmpty(userModel.EmailAddress))
            {
                if (userLogins.ContainsKey(userModel.EmailAddress) && userLogins[userModel.EmailAddress].Password == userModel.Password)
                {
                    HttpContext.Session.SetString("UserName", userLogins[userModel.EmailAddress].EmailAddress);
                    return Redirect("/Home/Index");
                }
            }
            return Redirect("/Home/Index");
        }
    }
}

using System;
using DataRoom.Models;
using DataRoom.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataRoom.Controllers
{
    public class AuthController : Controller
    {

        private readonly UserService userService;
        private readonly AuthService authService;
        public AuthController(UserService uService, AuthService aService)
        {
            userService = uService;
            authService = aService;
        }

        [Route("/Login")]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            ViewData["QuotingReference"] = authService.GetQuotingReference(HttpContext.Connection.RemoteIpAddress.ToString());
            return View();
        }

        [Route("/Logout")]
        public IActionResult Logout()
        {
            return Redirect("Login");
        }

        public IActionResult LoginPost(UserModel userModel)
        {
            if(authService.AuthenticateUser(userModel))
            {
                var user = userService.GetUserModel(userModel.Username);

                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserRole", user.Role.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

    }
}

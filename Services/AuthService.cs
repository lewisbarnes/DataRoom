using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataRoom.Models;
using Microsoft.Extensions.Configuration;

namespace DataRoom.Services
{
    public class AuthService
    {
        private readonly IConfiguration configuration;
        private readonly UserService userService;
        private readonly string accessRequestAddress;
        private readonly string quotingReferencePrefix;

        public AuthService(IConfiguration config, UserService uService)
        {
            configuration = config;
            userService = uService;
            quotingReferencePrefix = configuration.GetValue<string>("QuotingReferencePrefix");
            accessRequestAddress = configuration.GetValue<string>("AccessRequestEmailAddress");
        }

        public bool AuthenticateUser(UserModel userModel)
        {
            if (!string.IsNullOrEmpty(userModel.Username))
            {
                var user = userService.GetUserModel(userModel.Username);
                if (user != null && user.Password == userModel.Password)
                {
                    return true;
                }
            }
            return false;
        }

        // Gets the quoting reference displayed on the login page
        public string GetQuotingReference(string ipAddress)
        {
            return $"{quotingReferencePrefix}-{ipAddress.Replace('.', '-')}-{DateTime.Now:HH-mm-ss}";
        }
    }
}

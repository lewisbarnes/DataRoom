using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using DataRoom.Helpers;
using Newtonsoft.Json;

namespace DataRoom.Models
{
    public class UserModel
    {
        [Display(Name = "Username")]
        [JsonProperty("username")]
        public string Username { get; set; }
        [Display(Name = "Passcode")]
        [JsonProperty("password")]

        public string Password { get; set; }
        [JsonProperty("role")]

        public UserRole Role { get; set; }
        [JsonProperty("permissions")]
        public List<string> Permissions { get; set; }
        [JsonProperty("firstname")]

        public string FirstName { get; set; }
        [JsonProperty("lastname")]

        public string LastName { get; set; }
        [JsonIgnore]
        public string FullName { get => string.Join(' ', FirstName, LastName); }

        public bool HasPermission(string value)
        {
            return Permissions.Contains(value);
        }

        public bool IsAdministrator()
        {
            return Role == UserRole.Admin;
        }

        public bool HasRole(UserRole role)
        {
            return Role >= role;
        }
    }

    public enum UserRole
    {
        User,
        DataManager,
        Admin
    }
}

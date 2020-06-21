using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataRoom.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DataRoom.Services
{
    public class UserService
    {
        private readonly IConfiguration configuration;
        private readonly string dataSource;
        private readonly List<UserModel> userLogins;

        public UserService(IConfiguration config)
        {
            configuration = config;
            dataSource = configuration.GetValue<string>("UserDataSource");
            userLogins = JsonConvert.DeserializeObject<List<UserModel>>(File.ReadAllText("userlogins.json"));
        }

        public UserModel GetUserModel(string username)
        {
            var ret = new UserModel();
            if(dataSource == "JSON")
            {
                ret = userLogins.FirstOrDefault(x => x.Username == username);
            } else
            {

            }
            return ret;
        }
    }
}

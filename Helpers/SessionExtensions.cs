using DataRoom.Models;
using Microsoft.AspNetCore.Http;

namespace DataRoom.Helpers
{
    public static class SessionExtensions
    {
        public static bool HasUsername(this ISession session)
        {
            return session.GetString("Username") != null;
        }

        public static bool IsAdministrator(this ISession session)
        {
            return session.GetString("UserRole") == "Admin";
        }
    }
}

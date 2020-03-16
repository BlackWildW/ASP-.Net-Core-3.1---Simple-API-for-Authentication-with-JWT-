using Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPassword(this IEnumerable<User> users)
        {
            if (users == null) return null;
            return users.Select(x => x.WithoutPassword());
        }
        public static User WithoutPassword(this User user)
        {
            if (user == null) return null;
            user.Password = null;
            user.ConfirmPassword = null;
            return user;
        }
    }
}

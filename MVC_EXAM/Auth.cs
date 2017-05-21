using MVC_EXAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
namespace MVC_EXAM
{
    public static class Auth
    {
        private const string Userkey = "MVC_EXAM.Auth.UserKey";

        public static User User
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return null;
                }
                var user = HttpContext.Current.Items[Userkey] as User;
                if (user==null)
                {
                    user = Database.Session.Query<User>().FirstOrDefault(x => x.Username == HttpContext.Current.User.Identity.Name);
                    if (user==null)
                    {
                        return null;
                    }
                    HttpContext.Current.Items[Userkey] = user;
                }
                return user;
            }
        }

    }
}
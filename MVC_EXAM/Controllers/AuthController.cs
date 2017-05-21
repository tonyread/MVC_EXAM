using MVC_EXAM.Models;
using MVC_EXAM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using System.Web.Security;
namespace MVC_EXAM.Controllers
{
    public class AuthController : Controller
    {
        //
        // GET: /Auth/
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AuthLogin form,string returnurl)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == form.Username);

            if (user==null)
            {//fakehash

                MVC_EXAM.Models.User.FakeHash();
            }
            if (user==null || !user.CheckPassword(form.Password))
            {
                 ModelState.AddModelError("Username", "Username or password is invalid !");
                 return View(form);
            }

            FormsAuthentication.SetAuthCookie(form.Username, true);
            if (!string.IsNullOrWhiteSpace(returnurl))
            {
                return Redirect(returnurl);
            }
            else
            {
                return RedirectToRoute("Home");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Home");
        }
        
	}
}
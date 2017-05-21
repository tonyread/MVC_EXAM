using MVC_EXAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using MVC_EXAM.Areas.Admin.ViewModels;
using MVC_EXAM.InfraStructure;
namespace MVC_EXAM.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTabAttribute("users")]
    public class UsersController : Controller
    {
        //
        // GET: /Admin/Users/

        private void SyncRoles(IList<RoleCheckbox>checkboxes,IList<Role>roles)
        {
            var selectedRoles = new List<Role>();

            foreach (var role in Database.Session.Query<Role>())
            {
                var checkbox = checkboxes.Single(x => x.Id==role.Id);
                checkbox.Name = role.Name;
                if (checkbox.IsChecked)
                {
                    selectedRoles.Add(role);
                }
            }

            foreach (var toAdd in selectedRoles.Where(t=>!roles.Contains(t)))
            {
                roles.Add(toAdd);
            }
            foreach (var toRemove in roles.Where(t=>!selectedRoles.Contains(t)).ToList())
            {
                roles.Remove(toRemove);
            }

        }
        public ActionResult Index()
        {
            var users = Database.Session.Query<User>();
            return View(new UsersIndex() { Users=users});
        }

        public ActionResult New()
        {

            return View(new UsersNew()
            {
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckbox { 
                
                 Id=role.Id,
                 Name=role.Name,
                 IsChecked=false
                }).ToList()
            });
        }
        [HttpPost]
        public ActionResult New(UsersNew form)
        {
            var user = new User();
            SyncRoles(form.Roles, user.Roles);
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            if (Database.Session.Query<User>().Any(x=>x.Username==form.Username))
            {
                ModelState.AddModelError("Username", "Username must be unique");
                return View(form);
            }
            user.Username = form.Username;
            user.PasswordSet(form.Password);
            user.Email = form.Email;

            Database.Session.Save(user);
            Database.Session.Flush();
            return RedirectToAction("index");
        }

        public ActionResult Edit(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user==null)
            {
                return HttpNotFound();
            }

            return View(new
                UsersEdit()
                {
                    Username = user.Username,
                    Email = user.Email,
                    Roles = Database.Session.Query<Role>().Select(role => new RoleCheckbox() { 
                    
                     Id=role.Id,
                     Name=role.Name,
                     IsChecked=user.Roles.Contains(role)
                    }).ToList()
                });
        }

        [HttpPost]
        public ActionResult Edit(int id,UsersEdit form)
        {
           
            if (!ModelState.IsValid)
            {
                return View(form);
            }
             
            if (Database.Session.Query<User>().Any(x=>x.Username==form.Username && x.Id!=id))
            {
                ModelState.AddModelError("Username", "Username must be unique");
                return View(form);
            }
            var user = Database.Session.Load<User>(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            SyncRoles(form.Roles, user.Roles);
            user.Username = form.Username;
            user.Email = form.Email;

            Database.Session.Update(user);
            Database.Session.Flush();

            return RedirectToAction("index");
        }

        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(new
                UsersResetPassword()
                {
                    Username = user.Username
                });
        }


        [HttpPost]
        public ActionResult ResetPassword(int id,UsersResetPassword form)
        {

            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.PasswordSet(form.Password);

            Database.Session.Update(user);
            Database.Session.Flush();
            return RedirectToAction("index");
        }

        public ActionResult Delete(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            Database.Session.Delete(user);
            Database.Session.Flush();

            return RedirectToAction("index");
        }
	}
}
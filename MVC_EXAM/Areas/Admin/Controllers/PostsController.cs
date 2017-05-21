using MVC_EXAM.Areas.Admin.ViewModels;
using MVC_EXAM.InfraStructure;
using MVC_EXAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
namespace MVC_EXAM.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTabAttribute("posts")]
    public class PostsController : Controller
    {
        private const int PostPerPage=10;
        public ActionResult Index(int page=1)
        {
            if (page<1)
            {
                return HttpNotFound();
            }
            var TotalCount=Database.Session.Query<Post>().Count();

            var currentPage=Database.Session.Query<Post>()
                .OrderByDescending(x=>x.CreatedAt)
                .Skip((page-1)*PostPerPage)
                .Take(PostPerPage).ToList();


            return View(new PostsIndex() { 
               Posts=new PagedData<Post>(currentPage,TotalCount,page,PostPerPage)
            });
        }

        public ActionResult New()
        {
            return View("Form", new PostsForm() { 
            IsNew=true
            });
        }
        public ActionResult Trash(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post==null)
            {
                return HttpNotFound();
            }
            post.DeletedAt = DateTime.Now;
            Database.Session.Update(post);
            return RedirectToAction("index");
        }
        public ActionResult Restore(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.DeletedAt = null;
            Database.Session.Update(post);
            return RedirectToAction("index");
        }

        public ActionResult Remove(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            
            Database.Session.Delete(post);
            Database.Session.Flush();
            return RedirectToAction("index");
        }
        public ActionResult Edit(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post==null)
            {
                return HttpNotFound();
            }
            return View("Form", new PostsForm()
            {
                IsNew = false,
                Content=post.Content,
                Title=post.Title,
                Slug=post.Slug,
                PostId=post.Id

            });
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult Form(PostsForm form)
        {
            form.IsNew = form.PostId == null;

            if (!ModelState.IsValid)
                return View(form);

            Post post;

            if (form.IsNew)
            {
                post = new Post()
                {
                    Title = form.Title,
                    Slug = form.Slug,
                    Content = form.Content,
                    CreatedAt = DateTime.UtcNow,
                    User = Auth.User

                };
            }
            else
            {
                post = Database.Session.Load<Post>(form.PostId);

                if (post == null)
                {
                    return HttpNotFound();
                }

                post.Title = form.Title;
                post.Slug = form.Slug;
                post.Content = form.Content;
                post.UpdatedAt = DateTime.UtcNow;
            }

            Database.Session.SaveOrUpdate(post);

            Database.Session.Flush();
            return RedirectToAction("Index");
        }
	}
}
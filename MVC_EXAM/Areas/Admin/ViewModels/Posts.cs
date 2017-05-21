using MVC_EXAM.InfraStructure;
using MVC_EXAM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_EXAM.Areas.Admin.ViewModels
{
    public class PostsIndex
    {
        public PagedData<Post> Posts { get; set; }
    }

    public class PostsForm
    {

        public bool IsNew { get; set; }
        public int? PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required][DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [Required]
        public string Slug { get; set; }
    }
}
﻿using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_EXAM.Models
{
    public class Tag
    {

        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Slug { get; set; }
        public virtual IList<Post> Posts { get; set; }
    }

    public class TagMap : ClassMapping<Tag>
    {
        public TagMap()
        {
            Table("tags");
            Id(x => x.Id, x => x.Generator(Generators.Identity));
            Property(x => x.Title, x => x.NotNullable(true));
            Property(x => x.Slug, x => x.NotNullable(true));

            Bag(x => x.Posts, x =>
            {
                x.Key(y => y.Column("tag_id"));
                x.Table("post_tags");
            },y=>y.ManyToMany(c=>c.Column("post_id")));
        }
    }
}
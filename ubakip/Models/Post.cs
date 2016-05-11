using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public float Rating { get; set; }
        public float UserRating { get; set; }
        public MPAARating MPAARating { get; set; }
        public string Cover { get; set; }
        public Users Author { get; set; }
        public List<Page> Pages = new List<Page>();
        public List<Tag> Tags { get; set; }
    }
}
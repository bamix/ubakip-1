using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class Post
    {
        public Comix Comix { get; set; }
        public float Rating { get; set; }
        public float UserRating { get; set; }
        public MPAARating MPAARating { get; set; }
    }
}
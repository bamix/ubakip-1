using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public virtual ICollection<Comix> Comixes { get; set; }

        public Tag()
        {
            Comixes = new List<Comix>();        
        }
    }
}
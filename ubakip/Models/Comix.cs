using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class Comix
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public User Author { get; set; }

        public DateTime DateCreated { get; set; }

        public int MPAARatingId { get; set; }

        public virtual Page CoverPage { get; set; }

        public virtual ICollection<Page> Pages { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public Comix()
        {
            Pages = new List<Page>();
            Tags = new List<Tag>();
        }
    }
}
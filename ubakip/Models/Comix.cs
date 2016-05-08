using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class Comix
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public string Rating { get; set; }
    }
}
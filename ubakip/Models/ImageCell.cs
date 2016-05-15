using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class ImageCell
    {
        public int id { get; set; }

        public string src { get; set; } 
              
        public int isVideo { get; set; }

        public float scale { get; set; }

        public int rotate { get; set; }

        public float posX { get; set; }

        public float posY { get; set; }

        public string cellId { get; set; }

        public string height { get; set; }

        public string width { get; set; }

        public virtual Page Page { get; set; }
    }
}
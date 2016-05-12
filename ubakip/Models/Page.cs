using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class Page
    {
        public int Id { get; set; }
        public List<ImageCell> ImageCell { get; set; }
        public List<Cloud> Clouds { get; set; }
        public string TemplateName { get; set; }
        public string Background { get; set; }
        public string Preview { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class Cloud
    {       
        public string id { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public float posX { get; set; }
        public float posY { get; set; }
        public string width { get; set; }
        public string height { get; set; }       
    }
}
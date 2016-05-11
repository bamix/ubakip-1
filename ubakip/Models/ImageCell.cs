﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class ImageCell
    {
        public string id { get; set; }
        public string src { get; set; }
        public string dd { get; set; }
        public bool isVideo { get; set; }
        public float scale { get; set; }
        public int rotate { get; set; }
        public float posX { get; set; }
        public float posY { get; set; }
        public string cellId { get; set; }
        public string height { get; set; }
        public string width { get; set; }
    }
}
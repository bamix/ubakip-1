using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class MPAARating
    {
        public string Photo { get; set; }

        public string Description { get; set; }

        public static List<MPAARating> MPAARatings{get{ return GetRatings();}}

        private static List<MPAARating> GetRatings()
        {
            return new List<MPAARating>()
                {
                    new MPAARating() {Description  = "General audiences",
                        Photo = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/RATED_G.svg/70px-RATED_G.svg.png"},
                    new MPAARating() {Description  = "Parental guidance suggested",
                        Photo = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/bc/RATED_PG.svg/70px-RATED_PG.svg.png"},
                    new MPAARating() {Description  = "Parents strongly cautioned",
                        Photo = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c0/RATED_PG-13.svg/70px-RATED_PG-13.svg.png"},
                    new MPAARating() {Description  = "Restricted",
                        Photo = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7e/RATED_R.svg/70px-RATED_R.svg.png"},
                    new MPAARating() {Description  = "No One 17 & Under Admitted",
                        Photo = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/50/Nc-17.svg/70px-Nc-17.svg.png"}
                };
        }

    }
}
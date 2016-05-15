using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ubakip.Models
{
    public class UserProfile
    {
        public Users User { get; set; }

        public UserInfo UserInfo { get; set; }

        public List<Medal> Medals { get; set; }

        public List<Comment> Comments { get; set; }
        
        public UserProfile()
        {
            this.Medals = new List<Medal>();
            this.Comments = new List<Models.Comment>();
        }
    }
}
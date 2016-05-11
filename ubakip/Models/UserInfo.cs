using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ubakip.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public int UserID { get; set;}        

        public string About { get; set; }  
             
        public string FirstName { get; set; }

        public string LastName { get; set; }    

        public float Rating { get; set; }
    }
}
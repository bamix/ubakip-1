using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ubakip.Models
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
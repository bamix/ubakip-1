namespace ubakip
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UsersInfo
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "text")]
        public string About { get; set; }

        public double? Rating { get; set; }

        public virtual User User { get; set; }
    }
}

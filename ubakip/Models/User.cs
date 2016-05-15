namespace ubakip
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
       
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; }

        [StringLength(8)]
        public string Theme { get; set; }

        [Required]
        [StringLength(4)]
        public string Lang { get; set; }

        public int Role { get; set; }

        public string Photo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

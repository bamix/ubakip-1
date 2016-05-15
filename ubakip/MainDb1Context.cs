using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ubakip.Models;

namespace ubakip
{
    public class MainDbContext : DbContext
    {
        public MainDbContext()
             : base("DefaultConnection")
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Cloud> Clouds { get; set; }
        public DbSet<ImageCell> ImageCell { get; set; }
        public DbSet<Page> Pages { get; set; }

    }
}
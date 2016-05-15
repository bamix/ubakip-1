namespace ubakip
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Models;
    public partial class DataBaseConnection : DbContext
    {
        public DataBaseConnection()
            : base("name=DataBaseConnection")
        {
        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsersInfo> UsersInfoes { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Comix> Comixes { get; set; }
        public DbSet<Cloud> Clouds { get; set; }
        public DbSet<ImageCell> ImageCells { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

namespace ubakip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pages", "Comix_Id1", "dbo.Comixes");
            DropIndex("dbo.Pages", new[] { "Comix_Id1" });
            DropColumn("dbo.Pages", "Comix_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pages", "Comix_Id1", c => c.Int());
            CreateIndex("dbo.Pages", "Comix_Id1");
            AddForeignKey("dbo.Pages", "Comix_Id1", "dbo.Comixes", "Id");
        }
    }
}

namespace ubakip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "Comix_Id1", c => c.Int());
            CreateIndex("dbo.Pages", "Comix_Id1");
            AddForeignKey("dbo.Pages", "Comix_Id1", "dbo.Comixes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pages", "Comix_Id1", "dbo.Comixes");
            DropIndex("dbo.Pages", new[] { "Comix_Id1" });
            DropColumn("dbo.Pages", "Comix_Id1");
        }
    }
}

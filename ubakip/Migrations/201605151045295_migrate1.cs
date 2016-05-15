namespace ubakip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clouds",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        type = c.String(),
                        text = c.String(),
                        posX = c.Single(nullable: false),
                        posY = c.Single(nullable: false),
                        width = c.String(),
                        height = c.String(),
                        Page_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Pages", t => t.Page_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TemplateName = c.String(),
                        Background = c.String(),
                        Preview = c.String(),
                        Comix_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comixes", t => t.Comix_Id)
                .Index(t => t.Comix_Id);
            
            CreateTable(
                "dbo.ImageCells",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        src = c.String(),
                        isVideo = c.Int(nullable: false),
                        scale = c.Single(nullable: false),
                        rotate = c.Int(nullable: false),
                        posX = c.Single(nullable: false),
                        posY = c.Single(nullable: false),
                        cellId = c.String(),
                        height = c.String(),
                        width = c.String(),
                        Page_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Pages", t => t.Page_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.Comixes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        MPAARatingId = c.Int(nullable: false),
                        Author_Id = c.Int(),
                        CoverPage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Author_Id)
                .ForeignKey("dbo.Pages", t => t.CoverPage_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.CoverPage_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(maxLength: 200),
                        Login = c.String(nullable: false, maxLength: 100),
                        Theme = c.String(maxLength: 8),
                        Lang = c.String(nullable: false, maxLength: 4),
                        Role = c.Int(nullable: false),
                        Photo = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Count = c.Int(nullable: false),
                        Comix_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comixes", t => t.Comix_Id)
                .Index(t => t.Comix_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        FromUser_Id = c.Int(),
                        ToUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FromUser_Id)
                .ForeignKey("dbo.Users", t => t.ToUser_Id)
                .Index(t => t.FromUser_Id)
                .Index(t => t.ToUser_Id);
            
            CreateTable(
                "dbo.UsersInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        About = c.String(unicode: false, storeType: "text"),
                        Rating = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersInfoes", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "ToUser_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "FromUser_Id", "dbo.Users");
            DropForeignKey("dbo.Tags", "Comix_Id", "dbo.Comixes");
            DropForeignKey("dbo.Pages", "Comix_Id", "dbo.Comixes");
            DropForeignKey("dbo.Comixes", "CoverPage_Id", "dbo.Pages");
            DropForeignKey("dbo.Comixes", "Author_Id", "dbo.Users");
            DropForeignKey("dbo.ImageCells", "Page_Id", "dbo.Pages");
            DropForeignKey("dbo.Clouds", "Page_Id", "dbo.Pages");
            DropIndex("dbo.UsersInfoes", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "ToUser_Id" });
            DropIndex("dbo.Comments", new[] { "FromUser_Id" });
            DropIndex("dbo.Tags", new[] { "Comix_Id" });
            DropIndex("dbo.Comixes", new[] { "CoverPage_Id" });
            DropIndex("dbo.Comixes", new[] { "Author_Id" });
            DropIndex("dbo.ImageCells", new[] { "Page_Id" });
            DropIndex("dbo.Pages", new[] { "Comix_Id" });
            DropIndex("dbo.Clouds", new[] { "Page_Id" });
            DropTable("dbo.UsersInfoes");
            DropTable("dbo.Comments");
            DropTable("dbo.Tags");
            DropTable("dbo.Users");
            DropTable("dbo.Comixes");
            DropTable("dbo.ImageCells");
            DropTable("dbo.Pages");
            DropTable("dbo.Clouds");
        }
    }
}

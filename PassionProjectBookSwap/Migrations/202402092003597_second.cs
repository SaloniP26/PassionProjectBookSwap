namespace PassionProjectBookSwap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "UserID");
            AddForeignKey("dbo.Books", "UserID", "dbo.NewUsers", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "UserID", "dbo.NewUsers");
            DropIndex("dbo.Books", new[] { "UserID" });
            DropColumn("dbo.Books", "UserID");
        }
    }
}

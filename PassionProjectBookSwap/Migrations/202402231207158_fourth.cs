namespace PassionProjectBookSwap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "GenreID", c => c.Int(nullable: false));
            AddColumn("dbo.Genres", "Genre_GenreId", c => c.Int());
            CreateIndex("dbo.Books", "GenreID");
            CreateIndex("dbo.Genres", "Genre_GenreId");
            AddForeignKey("dbo.Genres", "Genre_GenreId", "dbo.Genres", "GenreId");
            AddForeignKey("dbo.Books", "GenreID", "dbo.Genres", "GenreId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.Genres", "Genre_GenreId", "dbo.Genres");
            DropIndex("dbo.Genres", new[] { "Genre_GenreId" });
            DropIndex("dbo.Books", new[] { "GenreID" });
            DropColumn("dbo.Genres", "Genre_GenreId");
            DropColumn("dbo.Books", "GenreID");
        }
    }
}

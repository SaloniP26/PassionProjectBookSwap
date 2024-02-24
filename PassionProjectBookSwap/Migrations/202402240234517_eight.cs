namespace PassionProjectBookSwap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eight : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Genres", "Genre_GenreID", "dbo.Genres");
            DropIndex("dbo.Genres", new[] { "Genre_GenreID" });
            DropColumn("dbo.Genres", "Genre_GenreID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Genres", "Genre_GenreID", c => c.Int());
            CreateIndex("dbo.Genres", "Genre_GenreID");
            AddForeignKey("dbo.Genres", "Genre_GenreID", "dbo.Genres", "GenreID");
        }
    }
}

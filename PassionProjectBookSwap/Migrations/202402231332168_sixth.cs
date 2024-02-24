namespace PassionProjectBookSwap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sixth : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Genres", new[] { "Genre_GenreId" });
            CreateIndex("dbo.Genres", "Genre_GenreID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Genres", new[] { "Genre_GenreID" });
            CreateIndex("dbo.Genres", "Genre_GenreId");
        }
    }
}

namespace BookWorms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeReleaseDateToDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "ReleaseDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "ReleaseDate", c => c.DateTime(nullable: false));
        }
    }
}

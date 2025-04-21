namespace BookWorms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Content");
        }
    }
}

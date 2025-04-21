namespace BookWorms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialReadBooks : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReadBooks", "Rating", c => c.Double(nullable: false));
            AlterColumn("dbo.ReadBooks", "Type", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReadBooks", "Type", c => c.String(nullable: false));
            AlterColumn("dbo.ReadBooks", "Rating", c => c.Int(nullable: false));
        }
    }
}

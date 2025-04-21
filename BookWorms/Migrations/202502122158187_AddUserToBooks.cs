namespace BookWorms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserToBooks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FutureBooks", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.ReadBooks", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.FutureBooks", "UserId");
            CreateIndex("dbo.ReadBooks", "UserId");
            AddForeignKey("dbo.FutureBooks", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ReadBooks", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReadBooks", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FutureBooks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ReadBooks", new[] { "UserId" });
            DropIndex("dbo.FutureBooks", new[] { "UserId" });
            DropColumn("dbo.ReadBooks", "UserId");
            DropColumn("dbo.FutureBooks", "UserId");
        }
    }
}

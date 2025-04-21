namespace BookWorms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateFutureBooks1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FutureBooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        Genre = c.String(),
                        ImageUrl = c.String(),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FutureBooks", "BookId", "dbo.Books");
            DropIndex("dbo.FutureBooks", new[] { "BookId" });
            DropTable("dbo.FutureBooks");
        }
    }
}

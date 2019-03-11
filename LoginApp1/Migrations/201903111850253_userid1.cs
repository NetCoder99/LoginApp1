namespace LoginApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userid1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserId");
        }
    }
}

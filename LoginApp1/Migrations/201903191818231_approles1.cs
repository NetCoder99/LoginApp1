namespace LoginApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class approles1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "Description", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetRoles", "CreateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "CreateDate");
            DropColumn("dbo.AspNetRoles", "Description");
        }
    }
}

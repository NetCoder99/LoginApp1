namespace LoginApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_roleid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "RoleId", c => c.Int(identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "RoleId");
        }
    }
}

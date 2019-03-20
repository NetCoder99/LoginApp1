namespace LoginApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_roleid3 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AspNetRoles", "RoleId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetRoles", new[] { "RoleId" });
        }
    }
}

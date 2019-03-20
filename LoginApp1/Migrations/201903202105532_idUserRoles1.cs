namespace LoginApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idUserRoles1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserRoles", "UserRoleId", c => c.Int(identity: true));
            AddColumn("dbo.AspNetUserRoles", "CreateDate", c => c.DateTime());
            AddColumn("dbo.AspNetUserRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AspNetUserRoles", "UserRoleId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUserRoles", new[] { "UserRoleId" });
            DropColumn("dbo.AspNetUserRoles", "Discriminator");
            DropColumn("dbo.AspNetUserRoles", "CreateDate");
            DropColumn("dbo.AspNetUserRoles", "UserRoleId");
        }
    }
}

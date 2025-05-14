namespace eUseControl.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChanges : DbMigration
    {
        public override void Up()
        {
               AddColumn("dbo.UserMemberships", "MembershipExperationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserMemberships", "ExpirationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserMemberships", "MembershipExperationDate");
        }
    }
}

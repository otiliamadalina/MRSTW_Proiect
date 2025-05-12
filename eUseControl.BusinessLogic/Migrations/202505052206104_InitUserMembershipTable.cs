namespace eUseControl.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitUserMembershipTable : DbMigration
    {
        public override void Up()
        {
           CreateTable(
                "dbo.UserMemberships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MembershipType = c.String(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "UserMembershipID", c => c.Int());
            CreateIndex("dbo.Users", "UserMembershipID");
            AddForeignKey("dbo.Users", "UserMembershipID", "dbo.UserMemberships", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserMembershipID", "dbo.UserMemberships");
            DropIndex("dbo.Users", new[] { "UserMembershipID" });
            DropColumn("dbo.Users", "UserMembershipID");
            DropTable("dbo.UserMemberships");
        }
    }
}

namespace eUseControl.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitCoachesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
             "dbo.Coaches",
             c => new
             {
                 Id = c.Int(nullable: false, identity: true),
                 Name = c.String(nullable: false, maxLength: 50),
                 Surname = c.String(nullable: false, maxLength: 50),
                 Birthdate = c.DateTime(nullable: false),
             })
             .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Coaches");
        }
    }
}
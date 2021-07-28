namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpicfunctionality : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "LocationHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Locations", "PicExtension", c => c.String());
            AddColumn("dbo.Services", "ServiceHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "PicExtension");
            DropColumn("dbo.Services", "ServiceHasPic");
            DropColumn("dbo.Locations", "PicExtension");
            DropColumn("dbo.Locations", "LocationHasPic");
        }
    }
}

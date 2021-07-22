namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changebridge : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationxServices", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.LocationxServices", "ServiceID", "dbo.Services");
            DropIndex("dbo.LocationxServices", new[] { "LocationID" });
            DropIndex("dbo.LocationxServices", new[] { "ServiceID" });
            CreateTable(
                "dbo.ServiceLocations",
                c => new
                    {
                        Service_ServiceID = c.Int(nullable: false),
                        Location_LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Service_ServiceID, t.Location_LocationID })
                .ForeignKey("dbo.Services", t => t.Service_ServiceID, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_LocationID, cascadeDelete: true)
                .Index(t => t.Service_ServiceID)
                .Index(t => t.Location_LocationID);
            
            DropTable("dbo.LocationxServices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LocationxServices",
                c => new
                    {
                        LocationxServiceID = c.Int(nullable: false, identity: true),
                        LocationID = c.Int(nullable: false),
                        ServiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationxServiceID);
            
            DropForeignKey("dbo.ServiceLocations", "Location_LocationID", "dbo.Locations");
            DropForeignKey("dbo.ServiceLocations", "Service_ServiceID", "dbo.Services");
            DropIndex("dbo.ServiceLocations", new[] { "Location_LocationID" });
            DropIndex("dbo.ServiceLocations", new[] { "Service_ServiceID" });
            DropTable("dbo.ServiceLocations");
            CreateIndex("dbo.LocationxServices", "ServiceID");
            CreateIndex("dbo.LocationxServices", "LocationID");
            AddForeignKey("dbo.LocationxServices", "ServiceID", "dbo.Services", "ServiceID", cascadeDelete: true);
            AddForeignKey("dbo.LocationxServices", "LocationID", "dbo.Locations", "LocationID", cascadeDelete: true);
        }
    }
}

namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bridgetable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationxServices",
                c => new
                    {
                        LocationxServiceID = c.Int(nullable: false, identity: true),
                        LocationID = c.Int(nullable: false),
                        ServiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationxServiceID)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.LocationID)
                .Index(t => t.ServiceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocationxServices", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.LocationxServices", "LocationID", "dbo.Locations");
            DropIndex("dbo.LocationxServices", new[] { "ServiceID" });
            DropIndex("dbo.LocationxServices", new[] { "LocationID" });
            DropTable("dbo.LocationxServices");
        }
    }
}

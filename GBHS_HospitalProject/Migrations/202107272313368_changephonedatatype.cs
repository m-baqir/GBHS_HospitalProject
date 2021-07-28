namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changephonedatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "LocationPhone", c => c.String());
            AlterColumn("dbo.Services", "ServicePhone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "ServicePhone", c => c.Single(nullable: false));
            AlterColumn("dbo.Locations", "LocationPhone", c => c.Single(nullable: false));
        }
    }
}

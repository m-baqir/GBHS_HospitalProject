namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somependingchange : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Bookings");
            AddColumn("dbo.Specialists", "SpecialistFirstName", c => c.String());
            AddColumn("dbo.Specialists", "SpecialistLastName", c => c.String());
            AlterColumn("dbo.Bookings", "BookingID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Bookings", "BookingID");
            DropColumn("dbo.Specialists", "SpecialistName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Specialists", "SpecialistName", c => c.String());
            DropPrimaryKey("dbo.Bookings");
            AlterColumn("dbo.Bookings", "BookingID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Specialists", "SpecialistLastName");
            DropColumn("dbo.Specialists", "SpecialistFirstName");
            AddPrimaryKey("dbo.Bookings", "BookingID");
        }
    }
}

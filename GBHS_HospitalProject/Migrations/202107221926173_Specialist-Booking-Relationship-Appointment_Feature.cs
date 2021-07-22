namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpecialistBookingRelationshipAppointment_Feature : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Specialist_SpecialistID", c => c.Int());
            CreateIndex("dbo.Bookings", "Specialist_SpecialistID");
            AddForeignKey("dbo.Bookings", "Specialist_SpecialistID", "dbo.Specialists", "SpecialistID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "Specialist_SpecialistID", "dbo.Specialists");
            DropIndex("dbo.Bookings", new[] { "Specialist_SpecialistID" });
            DropColumn("dbo.Bookings", "Specialist_SpecialistID");
        }
    }
}

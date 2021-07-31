namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropBookingtable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients");
            DropForeignKey("dbo.Bookings", "Specialist_SpecialistID", "dbo.Specialists");
            DropIndex("dbo.Bookings", new[] { "Patient_PatientID" });
            DropIndex("dbo.Bookings", new[] { "Specialist_SpecialistID" });
            DropTable("dbo.Bookings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        BookingStartTime = c.DateTime(nullable: false),
                        BookingEndTime = c.DateTime(nullable: false),
                        BookingReasonToVisit = c.String(),
                        Patient_PatientID = c.Int(),
                        Specialist_SpecialistID = c.Int(),
                    })
                .PrimaryKey(t => t.BookingID);
            
            CreateIndex("dbo.Bookings", "Specialist_SpecialistID");
            CreateIndex("dbo.Bookings", "Patient_PatientID");
            AddForeignKey("dbo.Bookings", "Specialist_SpecialistID", "dbo.Specialists", "SpecialistID");
            AddForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients", "PatientID");
        }
    }
}

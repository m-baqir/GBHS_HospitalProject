namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropPatientBooking : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients");
            DropForeignKey("dbo.Bookings", "Specialist_SpecialistID", "dbo.Specialists");
            DropIndex("dbo.Bookings", new[] { "UserID" });
            DropIndex("dbo.Bookings", new[] { "Patient_PatientID" });
            DropIndex("dbo.Bookings", new[] { "Specialist_SpecialistID" });
            DropTable("dbo.Bookings");
            DropTable("dbo.Patients");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientID = c.Int(nullable: false, identity: true),
                        PatientFirstName = c.String(),
                        PatientLastName = c.String(),
                        PatientEmail = c.String(),
                        PatientPhoneNumber = c.String(),
                        PatientGender = c.String(),
                    })
                .PrimaryKey(t => t.PatientID);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        BookingStartTime = c.DateTime(nullable: false),
                        BookingEndTime = c.DateTime(nullable: false),
                        BookingReasonToVisit = c.String(),
                        UserID = c.String(maxLength: 128),
                        Patient_PatientID = c.Int(),
                        Specialist_SpecialistID = c.Int(),
                    })
                .PrimaryKey(t => t.BookingID);
            
            CreateIndex("dbo.Bookings", "Specialist_SpecialistID");
            CreateIndex("dbo.Bookings", "Patient_PatientID");
            CreateIndex("dbo.Bookings", "UserID");
            AddForeignKey("dbo.Bookings", "Specialist_SpecialistID", "dbo.Specialists", "SpecialistID");
            AddForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients", "PatientID");
            AddForeignKey("dbo.Bookings", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}

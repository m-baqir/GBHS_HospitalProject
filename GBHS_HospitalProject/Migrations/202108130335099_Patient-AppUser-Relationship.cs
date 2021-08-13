namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientAppUserRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        BookingStartTime = c.DateTime(nullable: false),
                        BookingEndTime = c.DateTime(nullable: false),
                        BookingReasonToVisit = c.String(),
                    })
                .PrimaryKey(t => t.BookingID);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientID = c.String(nullable: false, maxLength: 128),
                        PatientFirstName = c.String(),
                        PatientLastName = c.String(),
                        PatientEmail = c.String(),
                        PatientPhoneNumber = c.String(),
                        PatientGender = c.String(),
                    })
                .PrimaryKey(t => t.PatientID)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientID)
                .Index(t => t.PatientID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patients", "PatientID", "dbo.AspNetUsers");
            DropIndex("dbo.Patients", new[] { "PatientID" });
            DropTable("dbo.Patients");
            DropTable("dbo.Bookings");
        }
    }
}

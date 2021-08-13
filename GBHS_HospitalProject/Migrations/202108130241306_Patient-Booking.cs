namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientBooking : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients");
            DropIndex("dbo.Bookings", new[] { "UserID" });
            DropIndex("dbo.Bookings", new[] { "Patient_PatientID" });
            DropColumn("dbo.Bookings", "UserID");
            DropColumn("dbo.Bookings", "Patient_PatientID");
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
            
            AddColumn("dbo.Bookings", "Patient_PatientID", c => c.Int());
            AddColumn("dbo.Bookings", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bookings", "Patient_PatientID");
            CreateIndex("dbo.Bookings", "UserID");
            AddForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients", "PatientID");
            AddForeignKey("dbo.Bookings", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}

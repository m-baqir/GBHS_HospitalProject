namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientBookingRelationshipAppointment_Feature : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Patient_PatientID", c => c.Int());
            CreateIndex("dbo.Bookings", "Patient_PatientID");
            AddForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients", "PatientID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients");
            DropIndex("dbo.Bookings", new[] { "Patient_PatientID" });
            DropColumn("dbo.Bookings", "Patient_PatientID");
        }
    }
}

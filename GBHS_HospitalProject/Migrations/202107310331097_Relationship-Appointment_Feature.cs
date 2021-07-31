namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationshipAppointment_Feature : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Patient_PatientID", c => c.Int());
            AddColumn("dbo.Bookings", "Specialist_SpecialistID", c => c.Int());
            CreateIndex("dbo.Bookings", "Patient_PatientID");
            CreateIndex("dbo.Bookings", "Specialist_SpecialistID");
            AddForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients", "PatientID");
            AddForeignKey("dbo.Bookings", "Specialist_SpecialistID", "dbo.Specialists", "SpecialistID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "Specialist_SpecialistID", "dbo.Specialists");
            DropForeignKey("dbo.Bookings", "Patient_PatientID", "dbo.Patients");
            DropIndex("dbo.Bookings", new[] { "Specialist_SpecialistID" });
            DropIndex("dbo.Bookings", new[] { "Patient_PatientID" });
            DropColumn("dbo.Bookings", "Specialist_SpecialistID");
            DropColumn("dbo.Bookings", "Patient_PatientID");
        }
    }
}

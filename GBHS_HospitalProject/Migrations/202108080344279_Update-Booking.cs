namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBooking : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Bookings", name: "Patient_PatientID", newName: "PatientID");
            RenameColumn(table: "dbo.Bookings", name: "Specialist_SpecialistID", newName: "SpecialistID");
            RenameIndex(table: "dbo.Bookings", name: "IX_Patient_PatientID", newName: "IX_PatientID");
            RenameIndex(table: "dbo.Bookings", name: "IX_Specialist_SpecialistID", newName: "IX_SpecialistID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Bookings", name: "IX_SpecialistID", newName: "IX_Specialist_SpecialistID");
            RenameIndex(table: "dbo.Bookings", name: "IX_PatientID", newName: "IX_Patient_PatientID");
            RenameColumn(table: "dbo.Bookings", name: "SpecialistID", newName: "Specialist_SpecialistID");
            RenameColumn(table: "dbo.Bookings", name: "PatientID", newName: "Patient_PatientID");
        }
    }
}

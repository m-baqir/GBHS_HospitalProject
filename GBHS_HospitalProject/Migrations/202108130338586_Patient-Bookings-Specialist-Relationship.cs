namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientBookingsSpecialistRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "PatientID", c => c.String(maxLength: 128));
            AddColumn("dbo.Bookings", "SpecialistID", c => c.Int());
            CreateIndex("dbo.Bookings", "PatientID");
            CreateIndex("dbo.Bookings", "SpecialistID");
            AddForeignKey("dbo.Bookings", "PatientID", "dbo.Patients", "PatientID");
            AddForeignKey("dbo.Bookings", "SpecialistID", "dbo.Specialists", "SpecialistID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "SpecialistID", "dbo.Specialists");
            DropForeignKey("dbo.Bookings", "PatientID", "dbo.Patients");
            DropIndex("dbo.Bookings", new[] { "SpecialistID" });
            DropIndex("dbo.Bookings", new[] { "PatientID" });
            DropColumn("dbo.Bookings", "SpecialistID");
            DropColumn("dbo.Bookings", "PatientID");
        }
    }
}

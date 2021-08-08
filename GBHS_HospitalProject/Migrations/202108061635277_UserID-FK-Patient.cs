namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIDFKPatient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Patients", "UserID");
            AddForeignKey("dbo.Patients", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patients", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Patients", new[] { "UserID" });
            DropColumn("dbo.Patients", "UserID");
        }
    }
}

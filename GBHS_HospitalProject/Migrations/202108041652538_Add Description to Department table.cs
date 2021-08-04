namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptiontoDepartmenttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "Description", c => c.String());
            AddColumn("dbo.Specialists", "SpecialistFirstName", c => c.String());
            AddColumn("dbo.Specialists", "SpecialistLastName", c => c.String());
            DropColumn("dbo.Specialists", "SpecialistName");
    }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "Description");
            DropColumn("dbo.Specialists", "SpecialistFirstName");
            DropColumn("dbo.Specialists", "SpecialistLastName");
            DropColumn("dbo.Specialists", "SpecialistName");
    }
    }
}

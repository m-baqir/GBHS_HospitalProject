namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingdatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Specialists", "SpecialistFirstName", c => c.String());
            AlterColumn("dbo.Specialists", "SpecialistLastName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Specialists", "SpecialistLastName", c => c.String(nullable: false));
            AlterColumn("dbo.Specialists", "SpecialistFirstName", c => c.String(nullable: false));
        }
    }
}

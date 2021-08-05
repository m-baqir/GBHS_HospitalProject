namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Specialists", "SpecialistFirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Specialists", "SpecialistLastName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Specialists", "SpecialistLastName", c => c.String());
            AlterColumn("dbo.Specialists", "SpecialistFirstName", c => c.String());
        }
    }
}

namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Specialisttable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Specialists",
                c => new
                    {
                        SpecialistID = c.Int(nullable: false, identity: true),
                        SpecialistName = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SpecialistID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Specialists", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Specialists", new[] { "DepartmentID" });
            DropTable("dbo.Specialists");
        }
    }
}

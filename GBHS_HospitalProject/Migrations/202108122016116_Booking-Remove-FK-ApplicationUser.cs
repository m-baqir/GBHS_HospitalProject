namespace GBHS_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingRemoveFKApplicationUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "UserID" });
            DropColumn("dbo.Bookings", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bookings", "UserID");
            AddForeignKey("dbo.Bookings", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}

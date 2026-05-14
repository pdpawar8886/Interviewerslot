namespace Interviewerslot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interviewslot : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.AdminId);
            
            CreateTable(
                "dbo.InterviewBookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        SirId = c.Int(nullable: false),
                        AvailabilityId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        FromTime = c.Time(nullable: false, precision: 7),
                        ToTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.SirAvailabilities", t => t.AvailabilityId, cascadeDelete: true)
                .ForeignKey("dbo.Sirs", t => t.SirId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.SirId)
                .Index(t => t.AvailabilityId);
            
            CreateTable(
                "dbo.SirAvailabilities",
                c => new
                    {
                        AvailabilityId = c.Int(nullable: false, identity: true),
                        SirId = c.Int(nullable: false),
                        AvailableDate = c.DateTime(nullable: false),
                        FromTime = c.Time(nullable: false, precision: 7),
                        ToTime = c.Time(nullable: false, precision: 7),
                        IsBooked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AvailabilityId)
                .ForeignKey("dbo.Sirs", t => t.SirId)
                .Index(t => t.SirId);
            
            CreateTable(
                "dbo.Sirs",
                c => new
                    {
                        SirId = c.Int(nullable: false, identity: true),
                        SirName = c.String(),
                        Email = c.String(),
                        password = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.SirId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterviewBookings", "StudentId", "dbo.Students");
            DropForeignKey("dbo.InterviewBookings", "SirId", "dbo.Sirs");
            DropForeignKey("dbo.InterviewBookings", "AvailabilityId", "dbo.SirAvailabilities");
            DropForeignKey("dbo.SirAvailabilities", "SirId", "dbo.Sirs");
            DropIndex("dbo.SirAvailabilities", new[] { "SirId" });
            DropIndex("dbo.InterviewBookings", new[] { "AvailabilityId" });
            DropIndex("dbo.InterviewBookings", new[] { "SirId" });
            DropIndex("dbo.InterviewBookings", new[] { "StudentId" });
            DropTable("dbo.Students");
            DropTable("dbo.Sirs");
            DropTable("dbo.SirAvailabilities");
            DropTable("dbo.InterviewBookings");
            DropTable("dbo.Admins");
        }
    }
}

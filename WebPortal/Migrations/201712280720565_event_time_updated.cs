namespace WebPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class event_time_updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Time", c => c.String());
            DropColumn("dbo.Events", "From");
            DropColumn("dbo.Events", "To");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "To", c => c.DateTime(nullable: false));
            AddColumn("dbo.Events", "From", c => c.DateTime(nullable: false));
            DropColumn("dbo.Events", "Time");
        }
    }
}

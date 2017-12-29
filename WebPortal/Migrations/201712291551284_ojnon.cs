namespace WebPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ojnon : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Businesses", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Businesses", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Businesses", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Businesses", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.Businesses", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Businesses", "Image", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "Venue", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "Image", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Image", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Image", c => c.String());
            AlterColumn("dbo.Movies", "Description", c => c.String());
            AlterColumn("dbo.Movies", "Category", c => c.String());
            AlterColumn("dbo.Movies", "Title", c => c.String());
            AlterColumn("dbo.Events", "Image", c => c.String());
            AlterColumn("dbo.Events", "Description", c => c.String());
            AlterColumn("dbo.Events", "Venue", c => c.String());
            AlterColumn("dbo.Events", "Title", c => c.String());
            AlterColumn("dbo.Businesses", "Image", c => c.String());
            AlterColumn("dbo.Businesses", "Description", c => c.String());
            AlterColumn("dbo.Businesses", "Category", c => c.String());
            AlterColumn("dbo.Businesses", "Address", c => c.String());
            AlterColumn("dbo.Businesses", "City", c => c.String());
            AlterColumn("dbo.Businesses", "Name", c => c.String());
        }
    }
}

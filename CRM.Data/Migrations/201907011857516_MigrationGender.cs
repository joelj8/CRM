namespace CRM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationGender : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        GenderId = c.String(nullable: false, maxLength: 128),
                        GenderName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GenderId);
            
            AddColumn("dbo.Customers", "GenderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Customers", "GenderId");
            AddForeignKey("dbo.Customers", "GenderId", "dbo.Genders", "GenderId");
            DropColumn("dbo.Customers", "Gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Gender", c => c.String());
            DropForeignKey("dbo.Customers", "GenderId", "dbo.Genders");
            DropIndex("dbo.Customers", new[] { "GenderId" });
            DropColumn("dbo.Customers", "GenderId");
            DropTable("dbo.Genders");
        }
    }
}

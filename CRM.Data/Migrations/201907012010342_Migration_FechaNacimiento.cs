namespace CRM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_FechaNacimiento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "fechaNacimiento", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "fechaNacimiento");
        }
    }
}

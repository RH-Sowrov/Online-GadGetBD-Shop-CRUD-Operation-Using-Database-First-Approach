namespace MVCProject.Migrations.GadgetDBDB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdbA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandId = c.Int(nullable: false, identity: true),
                        BrandName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.BrandId);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        PhoneId = c.Int(nullable: false, identity: true),
                        PhoneName = c.String(nullable: false, maxLength: 50),
                        ReliseDate = c.DateTime(nullable: false, storeType: "date"),
                        IsOfficial = c.Boolean(nullable: false),
                        Picture = c.String(),
                        PhoneModelId = c.Int(nullable: false),
                        BrandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhoneId)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.PhoneModels", t => t.PhoneModelId, cascadeDelete: true)
                .Index(t => t.PhoneModelId)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.PhoneModels",
                c => new
                    {
                        PhoneModelId = c.Int(nullable: false, identity: true),
                        ModelName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.PhoneModelId);
            
            CreateTable(
                "dbo.StockDetails",
                c => new
                    {
                        StockDetailId = c.Int(nullable: false, identity: true),
                        Color = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        PhoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockDetailId)
                .ForeignKey("dbo.Phones", t => t.PhoneId, cascadeDelete: true)
                .Index(t => t.PhoneId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockDetails", "PhoneId", "dbo.Phones");
            DropForeignKey("dbo.Phones", "PhoneModelId", "dbo.PhoneModels");
            DropForeignKey("dbo.Phones", "BrandId", "dbo.Brands");
            DropIndex("dbo.StockDetails", new[] { "PhoneId" });
            DropIndex("dbo.Phones", new[] { "BrandId" });
            DropIndex("dbo.Phones", new[] { "PhoneModelId" });
            DropTable("dbo.StockDetails");
            DropTable("dbo.PhoneModels");
            DropTable("dbo.Phones");
            DropTable("dbo.Brands");
        }
    }
}

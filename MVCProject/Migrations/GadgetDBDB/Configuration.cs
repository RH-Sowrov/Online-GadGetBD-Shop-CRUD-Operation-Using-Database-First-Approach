namespace MVCProject.Migrations.GadgetDBDB
{
    using MVCProject.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCProject.Models.GadgetDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\GadgetDBDB";
        }

        protected override void Seed(MVCProject.Models.GadgetDBContext db)
        {
            db.Brands.AddRange(new Brand[]
             {
               new Brand{BrandName="Samsung"},
               new Brand{BrandName="IPhone"},
               new Brand{BrandName="Xiaomi"}
             });
            db.PhoneModels.AddRange(new PhoneModel[]
            {
                new PhoneModel{ModelName="Galaxy S24 Ultra"},
                new PhoneModel{ModelName="15 Pro Max"},
                new PhoneModel{ModelName="Note 13"}

            });
            db.SaveChanges();
            Phone p = new Phone
            {
                PhoneName = "A73",
                PhoneModelId = 1,
                BrandId = 1,
                ReliseDate = new DateTime(2024, 02, 01),
                IsOfficial = true,
                Picture = "Phn.jpg"
            };
            p.StockDetails.Add(new StockDetail { Color = Color.Black, Price = 200000, Quantity = 20 });
            p.StockDetails.Add(new StockDetail { Color = Color.Lavender, Price = 205000, Quantity = 20 });
            db.Phones.Add(p);
            db.SaveChanges();
        }
    }
}

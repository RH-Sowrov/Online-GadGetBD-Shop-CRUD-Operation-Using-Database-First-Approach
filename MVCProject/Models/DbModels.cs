using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCProject.Models
{
    public enum Color { Black = 1, Violet, Red, Gold, White, Silver, SkyBlue, Grey, Lavender }
    public class PhoneModel
    {
        public int PhoneModelId { get; set; }
        [Required, StringLength(50), Display(Name = "Model Name")]
        public string ModelName { get; set; }
        public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
    public class Brand
    {
        public int BrandId { get; set; }
        [Required, StringLength(50), Display(Name = "Brand Name")]
        public string BrandName { get; set; }
        public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
    public class Phone
    {
        public int PhoneId { get; set; }
        [Required, StringLength(50), Display(Name = "Series")]
        public string PhoneName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Relise Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReliseDate { get; set; }
        public bool IsOfficial { get; set; }
        public string Picture { get; set; }
        [ForeignKey("PhoneModel")]
        public int PhoneModelId { get; set; }
        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public virtual PhoneModel PhoneModel { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<StockDetail> StockDetails { get; set; } = new List<StockDetail>();
    }
    public class StockDetail
    {
        public int StockDetailId { get; set; }
        public Color Color { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Required, ForeignKey("Phone")]
        public int PhoneId { get; set; }
        public virtual Phone Phone { get; set; }
    }
    public class GadgetDBContext : DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<PhoneModel> PhoneModels { get; set; }
        public DbSet<StockDetail> StockDetails { get; set; }
    }
}
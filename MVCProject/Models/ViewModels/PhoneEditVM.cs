using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProject.Models.ViewModels
{
    public class PhoneEditVM
    {
        public int PhoneId { get; set; }
        [Required, StringLength(50), Display(Name = "Series")]
        public string PhoneName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Relise Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReliseDate { get; set; }
        public bool IsOfficial { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        public int PhoneModelId { get; set; }
        public int BrandId { get; set; }
        public virtual List<StockDetail> StockDetails { get; set; } = new List<StockDetail>();
    }
}
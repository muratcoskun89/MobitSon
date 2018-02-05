namespace MobitBilismDgerlendirmeProjesi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Urunler")]
    public partial class Urunler
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan bo� b�rak�lamaz...")]
        [StringLength(500)]
        public string UrunAdi { get; set; }

        [StringLength(500)]
        public string UrunResimYolu { get; set; }
        [Required(ErrorMessage = "Bu alan bo� b�rak�lamaz...")]
        [AllowHtml]
        public string UrunAciklamasi { get; set; }
    }
}

namespace MobitBilismDgerlendirmeProjesi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanicilar")]
    public partial class Kullanicilar
    {
        public int id { get; set; }

        [Required]
        [StringLength(60)]
        public string AdiSoyadi { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Lütfen Email alanýný boþ brakmayýn ve geçerli bir Email adresi giriniz...")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Lütfen geçerli bir mail adresi giriniz...")]
        public string Email { get; set; }

        [MaxLength(8, ErrorMessage = "Þifre 8 Karakterden büyük Olamaz")]
        [MinLength(8, ErrorMessage = "Þifre 8 Karakterden Küçük Olamaz")]
        [Required(ErrorMessage = "Þifre 8 karakter olmalidir, Lütfen boþ brakmayýn ve eksik girmeyiniz...")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Required]
        [StringLength(50)]
        public string Yetkisi { get; set; }
    }
}

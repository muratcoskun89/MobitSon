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
        [Required(ErrorMessage = "L�tfen Email alan�n� bo� brakmay�n ve ge�erli bir Email adresi giriniz...")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "L�tfen ge�erli bir mail adresi giriniz...")]
        public string Email { get; set; }

        [MaxLength(8, ErrorMessage = "�ifre 8 Karakterden b�y�k Olamaz")]
        [MinLength(8, ErrorMessage = "�ifre 8 Karakterden K���k Olamaz")]
        [Required(ErrorMessage = "�ifre 8 karakter olmalidir, L�tfen bo� brakmay�n ve eksik girmeyiniz...")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Required]
        [StringLength(50)]
        public string Yetkisi { get; set; }
    }
}

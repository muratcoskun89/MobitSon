using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobitBilismDgerlendirmeProjesi.Models
{
    public class EmailModel
    {
        [StringLength(50)]
        [Required(ErrorMessage = "Lütfen Email alanını boş brakmayın ve geçerli bir Email adresi giriniz...")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Lütfen geçerli bir mail adresi giriniz...")]
        public string Kime { get; set; }
        [Required(ErrorMessage ="Başlık boş brakılamaz")]
        public string Baslik { get; set; }

        public string Icerik { get; set; }
      
    }
}
using System.ComponentModel.DataAnnotations;

namespace CodeFirstOrnek.Models
{
    public class Kitap
    {
        [Key]   //primary key için
        public int Id { get; set; }

        [Required] //kitap adı boş geçilemez
        public string kitapAdi { get; set; }

        [Required]
        public double fiyat { get; set; }

        [Required]
        public int sayfaSayisi { get; set; }
    }
}

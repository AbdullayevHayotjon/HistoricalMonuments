using System.ComponentModel.DataAnnotations;

namespace Obidalar.Models
{
    public class Obida
    {
        public int Id { get; set; }

        [Required]
        public string Nomi { get; set; }

        [Required]
        public string Viloyat { get; set; }

        public int Yil { get; set; }

        [Required]
        public string Tavsif { get; set; }
        [Required]
        public string RasmUrl { get; set; }
        [Required]
        public string XaritaUrl { get; set; }

        public List<Sharh> Sharhlar { get; set; } = new();
    }

}

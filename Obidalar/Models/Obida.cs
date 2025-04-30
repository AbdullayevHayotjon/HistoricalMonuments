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

        public string? RasmUrl { get; set; }
        public List<Sharh> Sharhlar { get; set; } = new();
    }

}

using System.ComponentModel.DataAnnotations;

namespace Obidalar.Models
{
    public class Obida
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nomi { get; set; }

        [Required]
        public string Viloyat { get; set; }

        public int Yil { get; set; }

        [Required]
        public string Tavsif { get; set; }

        [Required]
        public string XaritaUrl { get; set; }

        public int ViewCount { get; set; } = 0;

        public double Rating { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Sharh> Sharhlar { get; set; } = new();

        public List<User> LikedUsers { get; set; } = new();

        public List<ObidaMedia> Medialar { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;

namespace Obidalar.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Parol { get; set; }

        [Required]
        public string Ism { get; set; }

        public UserRole Role { get; set; } = UserRole.User; // Default: oddiy foydalanuvchi

        public List<Sharh> Sharhlar { get; set; } = new();

        public List<Obida> LikedObidalar { get; set; } = new(); // Qiziqqan obidalar (yurakcha bosilgan)
    }
    public enum UserRole
    {
        User,
        Admin
    }

}

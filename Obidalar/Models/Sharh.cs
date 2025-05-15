using System.ComponentModel.DataAnnotations;

namespace Obidalar.Models
{
    public class Sharh
    {
        [Key]
        public int Id { get; set; }

        public int ObidaId { get; set; }
        public Obida Obida { get; set; }

        public int UserId { get; set; } // Kim yozgan
        public User User { get; set; }

        public string Matn { get; set; }
        public DateTime Sana { get; set; }
    }
}

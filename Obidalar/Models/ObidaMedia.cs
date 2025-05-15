using System.ComponentModel.DataAnnotations;

namespace Obidalar.Models
{
    public class ObidaMedia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MediaUrl { get; set; }

        [Required]
        public MediaType Type { get; set; } // Rasm yoki Video

        public int ObidaId { get; set; }
        public Obida Obida { get; set; }
    }

    public enum MediaType
    {
        Image,
        Video
    }
}

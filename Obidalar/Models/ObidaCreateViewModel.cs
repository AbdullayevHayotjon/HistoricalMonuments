namespace Obidalar.Models
{
    public class ObidaCreateViewModel
    {
        public string Nomi { get; set; }
        public string Viloyat { get; set; }
        public int YaratilganYil { get; set; }
        public string Tavsif { get; set; }
        public string XaritaUrl { get; set; }

        // Media ma'lumotlari
        public List<IFormFile> Medialar { get; set; } // Rasm yoki Video fayllarni olish
    }

}

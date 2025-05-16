namespace Obidalar.Models
{
    public class ObidaViewModelDetails
    {       
        public int Id { get; set; }
        public string Nomi { get; set; }
        public string Viloyat { get; set; }
        public int Yil { get; set; }
        public int ViewCount { get; set; }
        public double Rating { get; set; }
        public string XaritaURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Tavsif { get; set; }
        public List<Sharh> Sharhlar { get; set; }
        public List<ObidaMedia> Medialar { get; set; }
    }
}

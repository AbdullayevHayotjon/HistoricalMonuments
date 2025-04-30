namespace Obidalar.Models
{
    public class Sharh
    {
        public int Id { get; set; }
        public int ObidaId { get; set; }
        public string Matn { get; set; }
        public DateTime Sana { get; set; }

        public Obida Obida { get; set; }
    }

}

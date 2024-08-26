namespace MvcOrnek.Models
{
    public class Kitap : IDisposable
    {
        public int Id { get; set; }
        public string kitapAdi { get; set; }
        public double fiyat {  get; set; }
        public int sayfaSayisi { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

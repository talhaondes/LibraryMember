namespace KutuphaneUye.Data
{
    public class Kitap
    {
    public int KitapID { get; set; }  // Veritabanında birincil anahtar
    public string KitapAd { get; set; } =null!; // Kitap adı
    public string? KitapTur { get; set; }  // Kitap türü (örneğin; roman, bilim, tarih vb.)
    public int KitapStok { get; set; }  // Kitaın kütüphanedeki stok miktarı
    public ICollection<Kiralama> Kiralamalar { get; set; } = new List<Kiralama>();

    }
}
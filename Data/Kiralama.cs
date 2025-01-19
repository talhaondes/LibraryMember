namespace KutuphaneUye.Data
{
    public class Kiralama
    {
    public int KiralamaID { get; set; }  // Veritabanında birincil anahtar
    public int UyeID { get; set; }  // Üye kimliği (Yabancı anahtar)
    public int KitapID { get; set; }  // Ktap kimliği (Yabancı anahtar)
    public DateTime KiralamaTarihi { get; set; }  // Kitap kiralama tarihi
    public DateTime? IadeTarihi { get; set; }  // Kitap iade tarihi (null olabilir, henüz iade edilmemişse)
    
    // İlişkileri tanımlamak için 
    public Uye Uye { get; set; } =null!;
    public Kitap Kitap { get; set; } =null!;
    }
    
}
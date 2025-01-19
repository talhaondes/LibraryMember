namespace KutuphaneUye.Data
{   
    
    public class Uye
    {
    public int UyeID { get; set; }  // Veritabanında birincil anahtar
    public string? UyeAd { get; set; }  // Üye adı
    public string? UyeSoyad { get; set; }  // Üye soyadı
    public string AdSoyad { get{return this.UyeAd+" "+this.UyeSoyad;} }
    public string UyeEposta { get; set; }  =null!; // Üye e-posta adresi
    public string UyeTelefon { get; set; }   =null!;// Üye telefon numarası


    public ICollection<Kiralama> Kiralamalar { get; set; } = new List<Kiralama>();

    }

}

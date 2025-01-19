using Microsoft.EntityFrameworkCore;

namespace KutuphaneUye.Data
{
    public class KutuphaneContext : DbContext
    {  
        public KutuphaneContext(DbContextOptions<KutuphaneContext> options)
        : base(options)
        {
        }
        public DbSet<Kitap> Kitaplar => Set<Kitap>();
        public DbSet<Uye> Uyeler => Set<Uye>();
        public DbSet<Kiralama> Kiralamalar  => Set<Kiralama>();

    }
}
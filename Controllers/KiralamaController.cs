using System.Threading.Tasks;
using KutuphaneUye.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KutuphaneUye.Controllers
{
    public class KiralamaController:Controller
    {
        private readonly KutuphaneContext _context;
        public KiralamaController(KutuphaneContext context)
        {
            _context=context;
        }
        public async Task<IActionResult> Index()
        {
            var kitapkayitlari=await _context.Kiralamalar.Include(u=>u.Uye).Include(k=>k.Kitap).ToListAsync();
            return View(kitapkayitlari);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Üye verilerini dropdown için hazırla
            ViewBag.Uyeler = new SelectList(await _context.Uyeler.ToListAsync(), "UyeID", "AdSoyad");

            //  stokta olan kitapları filtrele
            var stokkitap = await _context.Kitaplar.Where(k => k.KitapStok > 0).ToListAsync();   //stokta varsa göster

            ViewBag.Kitaplar = new SelectList(stokkitap, "KitapID", "KitapAd");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Kiralama model)
        {
            // Seçilen kitap ve üye bilgilerini al
            var kitap = await _context.Kitaplar.FindAsync(model.KitapID);
            var uye = await _context.Uyeler.FindAsync(model.UyeID);

            // Kitap ve üye var mı kontrol et
            if (kitap == null || uye == null)
            {
                return NotFound();
            }
        
            // Kitap stok kontrolü
            if (kitap.KitapStok <= 0)
            {
                // Eğer kitap stokta yoksa, kullanıcıya uyarı göster
                ModelState.AddModelError("", "Seçilen kitap mevcut değil.");
                ViewBag.Uyeler = new SelectList(await _context.Uyeler.ToListAsync(), "UyeID", "AdSoyad");
                ViewBag.Kitaplar = new SelectList(await _context.Kitaplar.ToListAsync(), "KitapID", "KitapAd");
                return View(model);
            }
            // Kiralama kaydını ekle
            _context.Kiralamalar.Add(model);
            // Kitap stok miktarını 1 azalt
            kitap.KitapStok -= 1;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {   
            if (id == null)
            {
                return NotFound();  // Eğer id null ise, kaydı bulamayız
            }

            var kira = await _context.Kiralamalar.FindAsync(id);  // Kiralama kaydını bul
            if (kira == null)
            {
                return NotFound();  // Kiralama kaydını bulamazsak
            }

            return View(kira);  // Kiralama kaydını View'a gönder
        }

        //default gelen id ile bizim id mizin karışmaması için from form 
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            // Kiralama kaydını bul
            var kira = await _context.Kiralamalar.FindAsync(id);
            if (kira == null)
            {
                return NotFound();  // Kiralama kaydını bulamazsak
            }

            // Kiralama kaydında hangi kitap ve üye olduğunu al
            var kitap = await _context.Kitaplar.FindAsync(kira.KitapID);
            if (kitap != null)
            {
                // Kitap stok miktarını 1 artır
                kitap.KitapStok += 1;
            }

            // Kiralama kaydını sil
            _context.Kiralamalar.Remove(kira);
            await _context.SaveChangesAsync();  // await== bu işlem bitmeden geçme alt satıra, Değişiklikleri kaydet ve geç

            return RedirectToAction("Index", "Kitap");  // Silme işleminden sonra Kitaplar sayfasına yönlendir
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();  // Eğer id null ise, kayıt bulunamaz
            }

            // Kiralama kaydını id'ye göre alıyoruz
            var kira = await _context.Kiralamalar
                .Include(k => k.Kitap)  // Kitap bilgilerini alıyoruz
                .Include(k => k.Uye)    // Üye bilgilerini alıyoruz
                .FirstOrDefaultAsync(k => k.KiralamaID == id);

            if (kira == null)
            {
                return NotFound();  // Eğer kiralama kaydını bulamazsak, hata dönüyoruz
            }

            return View(kira);  // Kiralama modelini View'a gönderiyoruz
        }
       [HttpPost]
        public async Task<IActionResult> Edit(int id, Kiralama model)
        {
            // Eğer id model ile eşleşmiyorsa
            if (id != model.KiralamaID)
            {
                return NotFound();
            }
            // Eğer ModelState geçerliyse
            if (!ModelState.IsValid)
            {
                try
                {
                    var kira = await _context.Kiralamalar
                        .FirstOrDefaultAsync(k => k.KiralamaID == model.KiralamaID);

                    if (kira == null)
                    {
                        return NotFound();
                    }

                    // Kiralama tarihini güncelliyoruz
                    kira.KiralamaTarihi = model.KiralamaTarihi;

                    // Veritabanında güncelleme yapıyoruz
                    _context.Update(kira);
                    await _context.SaveChangesAsync();

                    // İşlem başarılı ise yönlendirme yapıyoruz
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    // Eğer güncellenen kayıt bulunamazsa hata dönüyoruz
                    if (!_context.Kiralamalar.Any(u => u.KiralamaID == model.KiralamaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            // Eğer ModelState geçersizse, formu tekrar render ediyoruz
            return View(model);
        }


    }
}
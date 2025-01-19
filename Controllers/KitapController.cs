using KutuphaneUye.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KutuphaneUye.Controllers
{
    public class KitapController: Controller
    {
        private readonly KutuphaneContext _context;
        public KitapController(KutuphaneContext context)
        {
            _context=context;
        }
        public async Task<IActionResult> Index()
        {
            var kitaps=await _context.Kitaplar.ToListAsync();
            return View(kitaps);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Kitap model)
        {
            _context.Kitaplar.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {   
            var kitaps = await _context.Kitaplar.Include(k=>k.Kiralamalar).ThenInclude(k=>k.Uye).FirstOrDefaultAsync(k=>k.KitapID==id);
            if(id==null)
            {
                return NotFound();
            }
            if(kitaps==null)
            {
                return NotFound();
            }           
            return View(kitaps);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id,Kitap model)
        {
            if(id!=model.KitapID)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch(Exception)
                {
                    if(!_context.Kitaplar.Any(u=>u.KitapID==model.KitapID))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        //silmeden önce gelen sayfa
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {   
            var boook = await _context.Kitaplar.FindAsync(id);
            if(id==null)
            {
                return NotFound();
            }
            if(boook==null)
            {
                return NotFound();
            }           
            return View(boook);
        }
        //default gelen id ile bizim id mizin karışmaması için from form 
        [HttpPost]
        
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            var book1=await _context.Kitaplar.FindAsync(id); //kitabı sorgula eğer varsa getir yoksa notfound döndür
            if(book1==null)
            {
                return NotFound();
            }
            _context.Kitaplar.Remove(book1);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Kitap");
        }
    }
}
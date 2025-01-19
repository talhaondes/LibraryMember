using KutuphaneUye.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KutuphaneUye.Controllers
{
    public class UyeController: Controller
    {
        //controller içinde context nesnesini kullanmak için yaptık
        private readonly KutuphaneContext _context;
        public UyeController(KutuphaneContext context)
        {
            _context=context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Uyeler.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(Uye model)
        {
            _context.Uyeler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {   //Eğer first or default kullanırsak herhangi bir parametre ile arama yapabilirdik, şu durumda find ile sadece id ile yapabilriiz
            var uyee = await _context.Uyeler.Include(u=>u.Kiralamalar).ThenInclude(u=>u.Kitap).FirstOrDefaultAsync(u=>u.UyeID==id);
            if(id==null)
            {
                return NotFound();
            }
            if(uyee==null)
            {
                return NotFound();
            }           
            return View(uyee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //benim adıma b aşkası işlem yapmaması için koyduk
        public async Task<IActionResult> Edit(int id,Uye model)
        {
            if(id!=model.UyeID)
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
                    if(!_context.Uyeler.Any(u=>u.UyeID==model.UyeID))
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
            var uyee = await _context.Uyeler.FindAsync(id);
            if(id==null)
            {
                return NotFound();
            }
            if(uyee==null)
            {
                return NotFound();
            }           
            return View(uyee);
        }
        //default gelen id ile bizim id mizin karışmaması için from form 
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            var uyeee=await _context.Uyeler.FindAsync(id);
            if(uyeee==null)
            {
                return NotFound();
            }
            _context.Uyeler.Remove(uyeee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Uye");
        }
    }
}
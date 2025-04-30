using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obidalar.Data;
using Obidalar.Models;

namespace Obidalar.Controllers
{
    public class ObidaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ObidaController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(string search)
        {
            var obidalar = _context.Obidalar.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                obidalar = obidalar
                    .Where(o => o.Nomi.ToLower().Contains(search.ToLower()));
                ViewBag.SearchQuery = search;
            }

            // Asinxron ishlash uchun ToListAsync() metodidan foydalanamiz
            var result = await obidalar
    .Include(o => o.Sharhlar.OrderByDescending(s => s.Sana)) // Sharhlarni Sana bo'yicha saralash
    .ToListAsync();


            return View(result);
        }


        public async Task<IActionResult> Details(int id)
        {
            var obida = await _context.Obidalar
                .Include(o => o.Sharhlar.OrderByDescending(s => s.Sana)) // Sharhlar sanasi bo‘yicha kamayish tartibida
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obida == null)
                return NotFound();

            return View(obida);
        }


        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Obida obida, IFormFile Rasm)
        {
            if (Rasm != null)
            {
                var fileName = Path.GetFileName(Rasm.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "rasmlar", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Rasm.CopyToAsync(stream);
                }
                obida.RasmUrl = "/rasmlar/" + fileName;
            }

            _context.Add(obida);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Tahrirlash sahifasini ko‘rsatish
        public async Task<IActionResult> Edit(int id)
        {
            var obida = await _context.Obidalar.FindAsync(id);
            if (obida == null) return NotFound();
            return View(obida);
        }

        // Tahrirlash ma'lumotini qabul qilish
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Obida obida, IFormFile? Rasm)
        {
            if (id != obida.Id) return NotFound();

            if (Rasm != null)
            {
                var fileName = Path.GetFileName(Rasm.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "rasmlar", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Rasm.CopyToAsync(stream);
                }
                obida.RasmUrl = "/rasmlar/" + fileName;
            }

            _context.Update(obida);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // O'chirish sahifasini ko'rsatish
        public async Task<IActionResult> Delete(int id)
        {
            var obida = await _context.Obidalar.FindAsync(id);
            if (obida == null) return NotFound();
            return View(obida);
        }

        // O'chirishni tasdiqlash
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obida = await _context.Obidalar.FindAsync(id);
            if (obida == null) return NotFound();

            _context.Obidalar.Remove(obida);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SharhYoz(int obidaId, string matn)
        {
            if (!string.IsNullOrWhiteSpace(matn))
            {
                var sharh = new Sharh
                {
                    ObidaId = obidaId,
                    Matn = matn,
                    Sana = DateTime.UtcNow
                };
                _context.Sharhlar.Add(sharh);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = obidaId });
        }
    }
}

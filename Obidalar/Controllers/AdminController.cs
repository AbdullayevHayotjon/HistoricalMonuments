using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obidalar.Data;
using Obidalar.Models;

namespace Obidalar.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
            _context = context;
        }
        public async Task<IActionResult> Index(string filterBy, string search)
        {
            // Obida va uning tegishli ma'lumotlarini yuklash
            var obidalar = _context.Obidalar.AsQueryable();

            // Qidiruv va filtratsiya
            if (!string.IsNullOrEmpty(search))
            {
                if (filterBy == "nom")
                {
                    obidalar = obidalar.Where(o => o.Nomi.ToLower().Contains(search.ToLower()));
                }
                else if (filterBy == "viloyat")
                {
                    obidalar = obidalar.Where(o => o.Viloyat.ToLower().Contains(search.ToLower()));
                }
            }

            // Obidalarni olish
            var model = await obidalar
                .Select(o => new ObidaViewModel
                {
                    Id = o.Id,
                    Nomi = o.Nomi,
                    Viloyat = o.Viloyat,
                    Yil = o.Yil,
                    ViewCount = o.ViewCount,
                    Rating = o.Rating,
                    CreatedAt = o.CreatedAt,
                }).ToListAsync();

            return View(model);
        }



        public IActionResult Users()
        {
            return View();
        }
    }
}

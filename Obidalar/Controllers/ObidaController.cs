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

        public async Task<IActionResult> Index(string search, string filterBy)
        {
            var obidalar = _context.Obidalar.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                if (filterBy == "joylashuv")
                {
                    obidalar = obidalar
                        .Where(o => o.Viloyat.ToLower().Contains(search));
                }
                else // Default: nom
                {
                    obidalar = obidalar
                        .Where(o => o.Nomi.ToLower().Contains(search));
                }

                ViewBag.SearchQuery = search;
                ViewBag.FilterBy = filterBy;
            }

            var result = await obidalar
                .Include(o => o.Medialar)
                .Include(o => o.Sharhlar.OrderByDescending(s => s.Sana))
                .ToListAsync();

            return View(result);
        }


        public async Task<IActionResult> AllObida(string filterBy, string search)
        {
            var obidalar = _context.Obidalar
                .Include(o => o.Medialar)
                .AsQueryable();

            ViewBag.SearchQuery = search;
            ViewBag.FilterBy = filterBy;

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower(); // Kichik harfga o'tkazamiz

                if (filterBy == "joylashuv")
                {
                    obidalar = obidalar.Where(o => o.Viloyat.ToLower().Contains(search));
                }
                else // default: "nom"
                {
                    obidalar = obidalar.Where(o => o.Nomi.ToLower().Contains(search));
                }
            }


            return View(await obidalar.ToListAsync()); // Asinxron
        }



        public async Task<IActionResult> Details(int id)
        {


            var obida = await _context.Obidalar
                .Include(o => o.Sharhlar)
                    .ThenInclude(s => s.User) // Sharh ichidagi User'ni yuklaymiz
                .Include(o => o.Medialar)
                .FirstOrDefaultAsync(o => o.Id == id);

            obida.ViewCount += 1; // Har safar ko'rilganda view countni oshiramiz   

            await _context.SaveChangesAsync(); // O'zgarishlarni saqlaymiz

            if (obida == null)
            {
                return NotFound(); // Agar obida topilmasa
            }

            var model = new ObidaViewModelDetails
            {
                Id = obida.Id,
                Nomi = obida.Nomi,
                Viloyat = obida.Viloyat,
                Yil = obida.Yil,
                ViewCount = obida.ViewCount,
                Rating = obida.Rating,
                XaritaURL = obida.XaritaUrl,
                CreatedAt = obida.CreatedAt,
                Tavsif = obida.Tavsif,
                Sharhlar = obida.Sharhlar,
                Medialar = obida.Medialar
            };

            return View(model);  // Modelni view-ga uzatish
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ObidaCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Obida ma’lumotlarini yaratish
                var obida = new Obida
                {
                    Nomi = model.Nomi,
                    Viloyat = model.Viloyat,
                    Yil = model.YaratilganYil,
                    Tavsif = model.Tavsif,
                    XaritaUrl = model.XaritaUrl,
                    Rating = 5
                };

                // 2. Obidani DB ga saqlash
                _context.Obidalar.Add(obida);
                await _context.SaveChangesAsync();

                // 3. Fayllarni yuklash va saqlash (agar mavjud bo‘lsa)
                if (model.Medialar != null && model.Medialar.Count > 0)
                {
                    var mediaFolder = Path.Combine(_env.WebRootPath, "media");
                    if (!Directory.Exists(mediaFolder))
                        Directory.CreateDirectory(mediaFolder);

                    foreach (var mediaFile in model.Medialar)
                    {
                        if (mediaFile.Length > 0)
                        {
                            // Fayl nomini unikal qilish
                            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(mediaFile.FileName);
                            var filePath = Path.Combine(mediaFolder, uniqueFileName);

                            // Faylni serverga saqlash
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await mediaFile.CopyToAsync(stream);
                            }

                            // DB ga media yozuvini qo‘shish
                            var media = new ObidaMedia
                            {
                                MediaUrl = "/media/" + uniqueFileName,
                                Type = mediaFile.ContentType.Contains("video") ? MediaType.Video : MediaType.Image,
                                ObidaId = obida.Id
                            };
                            _context.ObidaMedialar.Add(media);
                        }
                    }

                    // Media fayllarini DB ga saqlash
                    await _context.SaveChangesAsync();
                }

                // 4. Muvaffaqiyatli qo‘shilgach, Index sahifasiga qaytish
                return RedirectToAction("Index", "Admin");
            }

            // Modelda xatolik bo‘lsa, formani yana ko‘rsatish
            return View(model);
        }


        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Index", "Login");

            var obida = await _context.Obidalar.FindAsync(id);
            if (obida == null) return NotFound();
            return View(obida);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Obida obida, IFormFile? Rasm)
        {
            if (id != obida.Id) return NotFound();

            var existingObida = await _context.Obidalar.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
            if (existingObida == null) return NotFound();

            if (Rasm != null)
            {
                var fileName = Path.GetFileName(Rasm.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "rasmlar", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Rasm.CopyToAsync(stream);
                }
                //obida.RasmUrl = "/rasmlar/" + fileName;
            }
            else
            {
                //obida.RasmUrl = existingObida.RasmUrl; // eski rasmni saqlab qolamiz
            }

            _context.Update(obida);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var obida = await _context.Obidalar.FindAsync(id);
            if (obida == null)
            {
                return NotFound();
            }

            _context.Obidalar.Remove(obida);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }


        [HttpPost]
        public async Task<IActionResult> SharhYozish(int ObidaId, string UserId, string Matn, int Rating)
        {
            if (string.IsNullOrWhiteSpace(Matn))
            {
                TempData["Xabar"] = "Sharh bo‘sh bo‘lmasligi kerak.";
                return RedirectToAction("Details", new { id = ObidaId });
            }
            var obida = await _context.Obidalar.FirstOrDefaultAsync(m => m.Id == ObidaId);
            if (Rating != 0)
                obida.Rating = (obida.Rating + Rating) / 2;

            var sharh = new Sharh
            {
                ObidaId = ObidaId,
                UserId = int.Parse(UserId),
                Matn = Matn,
                Sana = DateTime.UtcNow
            };

            _context.Sharhlar.Add(sharh);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = ObidaId });
        }

    }
}

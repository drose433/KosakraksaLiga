using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KosarkaskaLiga2019.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace KosarkaskaLiga2019.Controllers
{
    public class TimController : Controller
    {
        private readonly Kosarkaskaliga2019dbContext db;

        public TimController(Kosarkaskaliga2019dbContext _db)
        {
            db = _db;
        }
        public PartialViewResult _TraziTimove(string naziv, int id = 0)
        {
            
            IEnumerable<Tim> listaTimova = db.Timovi.Include(t => t.Grad);
            if (id != 0)
            {
                Grad g1 = db.Gradovi.Find(id);
                if (g1 != null)
                {
                    ViewBag.Grad = g1.NazivGrada;
                    listaTimova = listaTimova.Where(g => g.GradId == id);
                }
                else
                {
                    ViewBag.Grad = "";
                    return PartialView();
                }
            }
            else
            {
                ViewBag.Grad = "Svi timovi";
            }
            if (!string.IsNullOrWhiteSpace(naziv))
            {
                listaTimova = listaTimova
                .Where(t => t.Naziv.ToLower().Contains(naziv.ToLower()));
            }
            return PartialView(listaTimova.OrderByDescending(t=>t.BrojBodova));
        }
        public FileContentResult CitajSliku(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Tim t = db.Timovi.Find(id);

            if (t == null)
            {
                return null;
            }
            return File(t.Slika, t.TipSlike);
        }
        // GET: Tim
        public async Task<IActionResult> Index()
        {
            ViewBag.Gradovi = new SelectList(db.Gradovi, "GradId", "NazivGrada");
            var kosarkaskaliga2019dbContext = db.Timovi.Include(t => t.Grad).OrderByDescending(t=> t.BrojBodova);
            return View(await kosarkaskaliga2019dbContext.ToListAsync());
        }

        // GET: Tim/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tim = await db.Timovi
                .Include(t => t.Grad)
                .FirstOrDefaultAsync(m => m.TimId == id);
            if (tim == null)
            {
                return NotFound();
            }

            return View(tim);
        }

        // GET: Tim/Create
        [Authorize(Roles ="admin")]
        public IActionResult Create()
        {
            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "NazivGrada");
            return View();
        }

        // POST: Tim/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimId,Naziv,Slika,TipSlike,GradId,BrojBodova")] Tim tim, IFormFile odabranaSlika)
        {
            if (odabranaSlika == null)
            {
                ModelState.AddModelError("Slika", "Odaberi sliku");
            }
            if (ModelState.IsValid)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await odabranaSlika.CopyToAsync(ms);
                    tim.Slika = ms.ToArray();
                }
                tim.TipSlike = odabranaSlika.ContentType;
                db.Add(tim);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradId"] = new SelectList(db.Gradovi, "GradId", "NazivGrada", tim.GradId);
            return View(tim);
        }

        // GET: Tim/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tim = await db.Timovi.FindAsync(id);
            if (tim == null)
            {
                return NotFound();
            }
            ViewData["GradId"] = new SelectList(db.Gradovi, "GradId", "NazivGrada", tim.GradId);
            return View(tim);
        }

        // POST: Tim/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimId,Naziv,Slika,TipSlike,GradId,BrojBodova")] Tim tim)
        {
            if (id != tim.TimId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(tim);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimExists(tim.TimId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradId"] = new SelectList(db.Gradovi, "GradId", "NazivGrada", tim.GradId);
            return View(tim);
        }

        // GET: Tim/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tim = await db.Timovi
                .Include(t => t.Grad)
                .FirstOrDefaultAsync(m => m.TimId == id);
            if (tim == null)
            {
                return NotFound();
            }

            return View(tim);
        }

        // POST: Tim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tim = await db.Timovi.FindAsync(id);
            db.Timovi.Remove(tim);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimExists(int id)
        {
            return db.Timovi.Any(e => e.TimId == id);
        }
    }
}

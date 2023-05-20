using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KosarkaskaLiga2019.Models;
using Microsoft.AspNetCore.Authorization;

namespace KosarkaskaLiga2019.Controllers
{
    [Authorize]
    public class KomentarController : Controller
    {
        private readonly Kosarkaskaliga2019dbContext db;

        public KomentarController(Kosarkaskaliga2019dbContext _db)
        {
            db = _db;
        }

        // GET: Komentar
        public async Task<IActionResult> Index()
        {
            return View(await db.Komentari.ToListAsync());
        }

        // GET: Komentar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await db.Komentari
                .FirstOrDefaultAsync(m => m.Id == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // GET: Komentar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Komentar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClanId,Komentar1,Autor")] Komentar komentar)
        {
            if (ModelState.IsValid)
            {
                db.Add(komentar);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(komentar);
        }

        // GET: Komentar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await db.Komentari.FindAsync(id);
            if (komentar == null)
            {
                return NotFound();
            }
            return View(komentar);
        }

        // POST: Komentar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClanId,Komentar1,Autor")] Komentar komentar)
        {
            if (id != komentar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(komentar);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KomentarExists(komentar.Id))
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
            return View(komentar);
        }

        // GET: Komentar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await db.Komentari
                .FirstOrDefaultAsync(m => m.Id == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // POST: Komentar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var komentar = await db.Komentari.FindAsync(id);
            db.Komentari.Remove(komentar);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KomentarExists(int id)
        {
            return db.Komentari.Any(e => e.Id == id);
        }
    }
}

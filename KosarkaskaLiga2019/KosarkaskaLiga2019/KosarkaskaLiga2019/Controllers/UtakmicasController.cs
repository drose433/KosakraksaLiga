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
    public class UtakmicaController : Controller
    {
        private readonly Kosarkaskaliga2019dbContext db;

        public UtakmicaController(Kosarkaskaliga2019dbContext _db)
        {
            db = _db;
        }

        // GET: Utakmicas
        public async Task<IActionResult> Index()
        {
            var kosarkaskaliga2019dbContext = db.Utakmice.Include(u => u.Domacin).Include(u => u.Gost);
            return View(await kosarkaskaliga2019dbContext.ToListAsync());
        }

        // GET: Utakmicas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utakmica = await db.Utakmice
                .Include(u => u.Domacin)
                .Include(u => u.Gost)
                .FirstOrDefaultAsync(m => m.UtakmicaId == id);
            if (utakmica == null)
            {
                return NotFound();
            }

            return View(utakmica);
        }

        // GET: Utakmicas/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["DomacinId"] = new SelectList(db.Timovi, "TimId", "Naziv");
            ViewData["GostId"] = new SelectList(db.Timovi, "TimId", "Naziv");
            return View();
        }

        // POST: Utakmicas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UtakmicaId,DomacinId,GostId,Kolo,Rezultat")] Utakmica utakmica)
        {
            
            if (ModelState.IsValid)
            {
                db.Add(utakmica);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DomacinId"] = new SelectList(db.Timovi, "TimId", "Naziv", utakmica.DomacinId);
            ViewData["GostId"] = new SelectList(db.Timovi, "TimId", "Naziv", utakmica.GostId);
            return View(utakmica);
        }

        // GET: Utakmicas/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utakmica = await db.Utakmice.FindAsync(id);
            if (utakmica == null)
            {
                return NotFound();
            }
            
            ViewData["DomacinId"] = new SelectList(db.Timovi, "TimId", "Naziv", utakmica.DomacinId);
            ViewData["GostId"] = new SelectList(db.Timovi, "TimId", "Naziv", utakmica.GostId);
            return View(utakmica);
        }

        // POST: Utakmicas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UtakmicaId,DomacinId,GostId,Kolo,Rezultat")] Utakmica utakmica)
        {
            if (id != utakmica.UtakmicaId)
            {
                return NotFound();
            }
            if (utakmica.Rezultat != null)
            {
                string rezultat = utakmica.Rezultat;
                string[] rez = rezultat.Split(':');
                if (int.Parse(rez[0]) > int.Parse(rez[1]))
                {
                    Tim domacin = db.Timovi.Find(utakmica.DomacinId);
                    domacin.BrojBodova += 2;
                    db.Update(domacin);
                    await db.SaveChangesAsync();
                    Tim gost = db.Timovi.Find(utakmica.GostId);
                    gost.BrojBodova += 1;
                    db.Update(gost);
                    await db.SaveChangesAsync();
                }
                else 
                {
                    Tim domacin = db.Timovi.Find(utakmica.DomacinId);
                    domacin.BrojBodova += 1;
                    db.Update(domacin);
                    await db.SaveChangesAsync();
                    Tim gost = db.Timovi.Find(utakmica.GostId);
                    gost.BrojBodova += 2;
                    db.Update(gost);
                    await db.SaveChangesAsync();
                }
            }
           

            if (true)
            {

            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(utakmica);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtakmicaExists(utakmica.UtakmicaId))
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
            ViewData["DomacinId"] = new SelectList(db.Timovi, "TimId", "Naziv", utakmica.DomacinId);
            ViewData["GostId"] = new SelectList(db.Timovi, "TimId", "Naziv", utakmica.GostId);
            return View(utakmica);
        }

        // GET: Utakmicas/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utakmica = await db.Utakmice
                .Include(u => u.Domacin)
                .Include(u => u.Gost)
                .FirstOrDefaultAsync(m => m.UtakmicaId == id);
            if (utakmica == null)
            {
                return NotFound();
            }

            return View(utakmica);
        }

        // POST: Utakmicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utakmica = await db.Utakmice.FindAsync(id);
            db.Utakmice.Remove(utakmica);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtakmicaExists(int id)
        {
            return db.Utakmice.Any(e => e.UtakmicaId == id);
        }
    }
}

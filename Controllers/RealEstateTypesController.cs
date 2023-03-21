using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NWC.DAL.Database;
using NWC.DAL.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NWC.PL.Controllers
{
    public class RealEstateTypesController : Controller
    {
        private readonly NWCContext _context;

        public RealEstateTypesController(NWCContext context)
        {
            _context = context;
        }

        // GET: RealEstateTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.RealEstateTypes.ToListAsync());
        }

        // GET: RealEstateTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var realEstateTypes = await _context.RealEstateTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (realEstateTypes == null)
            {
                return NotFound();
            }

            return View(realEstateTypes);
        }

        // GET: RealEstateTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RealEstateTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Notes")] RealEstateTypes realEstateTypes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(realEstateTypes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(realEstateTypes);
            }
            catch (Exception)
            {

                return View(realEstateTypes);
            }

        }

        // GET: RealEstateTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var realEstateTypes = await _context.RealEstateTypes.FindAsync(id);
            if (realEstateTypes == null)
            {
                return NotFound();
            }
            return View(realEstateTypes);
        }

        // POST: RealEstateTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Notes")] RealEstateTypes realEstateTypes)
        {
            try
            {
                if (id != realEstateTypes.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(realEstateTypes);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RealEstateTypesExists(realEstateTypes.Id))
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
                return View(realEstateTypes);
            }
            catch (Exception)
            {

                return View(realEstateTypes);
            }
            
        }

        // GET: RealEstateTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var realEstateTypes = await _context.RealEstateTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (realEstateTypes == null)
            {
                return NotFound();
            }

            return View(realEstateTypes);
        }

        // POST: RealEstateTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var realEstateTypes = await _context.RealEstateTypes.FindAsync(id);
                _context.RealEstateTypes.Remove(realEstateTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            catch (Exception)
            {

                return RedirectToAction(nameof(Index));
            }
        }
            private bool RealEstateTypesExists(int id)
        {
            return _context.RealEstateTypes.Any(e => e.Id == id);
        }
    }
}

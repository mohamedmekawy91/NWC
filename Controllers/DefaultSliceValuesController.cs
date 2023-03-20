using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NWC.BL.Interfaces;
using NWC.DAL.Database;
using NWC.DAL.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NWC.PL.Controllers
{
    public class DefaultSliceValuesController : Controller
    {
        private readonly NWCContext _context;
        private readonly IGenericRepository<DefaultSliceValue> defaultSliceValueRepostiroy;

        public DefaultSliceValuesController(NWCContext context, IGenericRepository<DefaultSliceValue> defaultSliceValueRepostiroy)
        {
            _context = context;
            this.defaultSliceValueRepostiroy = defaultSliceValueRepostiroy;
        }

        // GET: DefaultSliceValues
        public async Task<IActionResult> Index()
        {
            var data = await defaultSliceValueRepostiroy.GetAll();
            return View(data);
        }

        // GET: DefaultSliceValues/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var defaultSliceValue = await defaultSliceValueRepostiroy.GetById(id);
            if (defaultSliceValue == null)
            {
                return NotFound();
            }

            return View(defaultSliceValue);
        }

        // GET: DefaultSliceValues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DefaultSliceValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( DefaultSliceValue defaultSliceValue)
        {
            if (ModelState.IsValid)
            {
                await defaultSliceValueRepostiroy.Add(defaultSliceValue);
                await defaultSliceValueRepostiroy.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(defaultSliceValue);
        }

        // GET: DefaultSliceValues/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            
            var defaultSliceValue = await defaultSliceValueRepostiroy.GetById(id);
            if (defaultSliceValue == null)
            {
                return NotFound();
            }
            return View(defaultSliceValue);
        }

        // POST: DefaultSliceValues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,DefaultSliceValue defaultSliceValue)
        {
            if (id != defaultSliceValue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    defaultSliceValueRepostiroy.Update(defaultSliceValue);
                    await defaultSliceValueRepostiroy.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DefaultSliceValueExists(defaultSliceValue.Id))
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
            return View(defaultSliceValue);
        }

        // GET: DefaultSliceValues/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var defaultSliceValue = await defaultSliceValueRepostiroy.GetById(id);
            if (defaultSliceValue == null)
            {
                return NotFound();
            }

            return View(defaultSliceValue);
        }

        // POST: DefaultSliceValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var defaultSliceValue = await defaultSliceValueRepostiroy.GetById(id);
            defaultSliceValueRepostiroy.Delete(defaultSliceValue);
            await defaultSliceValueRepostiroy.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool DefaultSliceValueExists(int id)
        {
            return _context.DefaultSliceValue.Any(e => e.Id == id);
        }
    }
}

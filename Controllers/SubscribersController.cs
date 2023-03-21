using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NWC.BL.Interfaces;
using NWC.DAL.Database;
using NWC.DAL.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NWC.PL.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly NWCContext _context;
        private readonly IGenericRepository<Subscriber> SubscriberRepository;

        public SubscribersController(NWCContext context, IGenericRepository<Subscriber> SubscriberRepository)
        {
            _context = context;
            this.SubscriberRepository = SubscriberRepository;
        }

        // GET: Subscriber
        public async Task<IActionResult> Index()
        {
            var data = await SubscriberRepository.GetAll();
            return View(data);
        }
        // GET: Subscriber Report
        public async Task<IActionResult> Report()
        {
            var data = await SubscriberRepository.GetAll();
            return View(data);
        }

        // GET: Subscriber/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var Subscriber = await SubscriberRepository.GetById(id);
            if (Subscriber == null)
            {
                return NotFound();
            }

            return View(Subscriber);
        }

        // GET: Subscriber/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subscriber/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subscriber Subscriber)
        {
            if (ModelState.IsValid)
            {
                await SubscriberRepository.Add(Subscriber);
                await SubscriberRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(Subscriber);
        }

        // GET: Subscriber/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View();
            }
            var Subscriber = await SubscriberRepository.GetById(id);
            if (Subscriber == null)
            {
                return NotFound();
            }
            return View(Subscriber);
        }

        // POST: Subscriber/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Subscriber Subscriber)
        {
            if (id != Subscriber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SubscriberRepository.Update(Subscriber);
                    await SubscriberRepository.Save();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SubscriberExists(Subscriber.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        //todo: logg ex 
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Subscriber);
        }

        // GET: Subscriber/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var Subscriber = await SubscriberRepository.GetById(id);
            if (Subscriber == null)
            {
                return NotFound();
            }

            return View(Subscriber);
        }

        // POST: Subscriber/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string  id)
        {
            var Subscriber = await SubscriberRepository.GetById(id);
            SubscriberRepository.Delete(Subscriber);
            await SubscriberRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriberExists(string id)
        {
            return _context.Subscriber.Any(e => e.Id == id);
        }
    }
}

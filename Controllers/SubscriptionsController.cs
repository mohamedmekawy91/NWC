using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NWC.BL.Repositories;
using NWC.DAL.Database;
using NWC.DAL.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NWC.PL.Controllers
{

    public class SubscriptionsController : Controller
    {
        private readonly NWCContext _context;
        private readonly SubscriberRepository subscriber;

        public SubscriptionsController(NWCContext context , SubscriberRepository subscriber)
        {
            _context = context;
            this.subscriber = subscriber;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subscription.Include("Subscriber").Include("RealEstateTypes").ToListAsync());
        }
        // GET: Subscriptions
        public async Task<IActionResult> Report()
        {
            return View(await _context.Subscription.Include("Subscriber").Include("RealEstateTypes").ToListAsync());
        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscription.Include("Subscriber").Include("RealEstateTypes")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            var RealStateModel =  _context.RealEstateTypes.ToList();
            ViewBag.RealStateList = new SelectList(RealStateModel, "Id", "Name");
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Subscription subscription)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _context.Subscriber.FindAsync(subscription.SubscriberId);
                    if (result.IsCompletedSuccessfully)
                    {
                        string date = DateTime.Now.ToString("MM-yy");

                        var lastsubscription = await _context.Subscription.OrderBy(x => x.Date).LastOrDefaultAsync();
                        if (lastsubscription == null)
                        {
                            subscription.Id = $"{1}-{date}";
                            _context.Add(subscription);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Report));
                        }
                        else
                        {
                            string lastId = lastsubscription.Id.Substring(0, lastsubscription.Id.IndexOf('-'));
                            var lastids = int.Parse(lastId) + 1;
                            subscription.Id = $"{lastids}-{date}";
                            _context.Add(subscription);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Report));

                        }
                    }

                        ModelState.AddModelError("","Subscriber ID Isn't correct");
                }
                var RealStateModel = _context.RealEstateTypes.ToList();
                ViewBag.RealStateList = new SelectList(RealStateModel, "Id", "Name");
                return View();
            }
            catch (Exception)
            {

                return View();
            }
            
        }

        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View();
            }
            var subscription = await _context.Subscription.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            var RealStateModel = await _context.RealEstateTypes.ToListAsync();
            ViewBag.RealStateList = new SelectList(RealStateModel, "Id", "Name");
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,RealEstateTypes,Subscriber,Unit_No,IsThereSanitation,Last_Reading_Meter,Notes")] Subscription subscription)
        {
            try
            {
                if (id != subscription.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(subscription);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SubscriptionExists(subscription.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    var RealStateModel = await _context.RealEstateTypes.ToListAsync();
                    ViewBag.RealStateList = new SelectList(RealStateModel, "Id", "Name");
                    return RedirectToAction(nameof(Index));
                }
                return View(subscription);
            }
            catch (Exception)
            {

                return View(subscription);
            }
            
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscription
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }
            var RealStateModel = await _context.RealEstateTypes.ToListAsync();
            ViewBag.RealStateList = new SelectList(RealStateModel, "Id", "Name");
            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var subscription = await _context.Subscription.FindAsync(id);
                _context.Subscription.Remove(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View();
            }
            
        }

        public JsonResult GetSubscriberInfoBySubscriberId(string SubId)
        {
            var model = subscriber.Get(a => a.Id == SubId);
            return Json(model);
        }
        private bool SubscriptionExists(string id)
        {
            return _context.Subscription.Any(e => e.Id == id);
        }
    }
}

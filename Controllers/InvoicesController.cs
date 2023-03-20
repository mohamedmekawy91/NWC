using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NWC.BL.Interfaces;
using NWC.BL.Repositories;
using NWC.DAL.Database;
using NWC.DAL.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NWC.PL.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly NWCContext _context;
        private readonly SubscriptionRepository Subscription;
        private readonly SubscriberRepository subscriber;
        private readonly InvoicesRepository Invoice;



        public InvoicesController(NWCContext context, SubscriptionRepository Subscription, SubscriberRepository subscriber, InvoicesRepository invoice)
        {
            _context = context;
            this.Subscription = Subscription;
            this.subscriber = subscriber;
            Invoice = invoice;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {


            return View(await _context.Invoices.Include(x=>x.Subscription).ToListAsync());
        }

        public  async Task<JsonResult> GetLastID()
        {
            string date = DateTime.Now.ToString("MM-yyyy");

            var lastInvoice = await _context.Invoices.OrderBy(x => x.Year).LastOrDefaultAsync();
            var lastId = lastInvoice.Id.Substring(0, lastInvoice.Id.IndexOf('-'));
            var lastids = int.Parse(lastId) + 1;
            var invoicesId = $"{lastids}-{date}";
            return Json(invoicesId);
        }
        // GET: Invoices Report
        public async Task<IActionResult> Report()
        {
            return View(await _context.Invoices.Include(x=>x.Subscription).ThenInclude(x=>x.Subscriber)
                .ToListAsync());
        }
        public  IActionResult Ask()
        {
             return  View();
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoices == null)
            {
                return NotFound();
            }

            return View(invoices);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year,Date,From,To,SubscriptionId,SubscriptionSubscriberId,Subscription.Subscriber.Name,Subscription.Last_Reading_Meter,PreviousConsumptionAmount,CurrentConsumptionAmount,ActualConsumptionAmount,ServiceFee,TaxRate,IsThereSanitation,Subscription.IsThereSanitation,Subscription.Unit_No,ConsumptionValue,SanitationConsumptionValue,TotalInvoice,TaxValue,TotalBill,Notes")] Invoices invoices)
        {
            if (ModelState.IsValid)
            {
                string date = DateTime.Now.ToString("MM-yyyy");

                var lastInvoice = await _context.Invoices.OrderBy(x => x.Year).LastOrDefaultAsync();
                if (lastInvoice == null)
                {
                    invoices.Id = $"{1}-{date}";
;
                    _context.Add(invoices);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Report));
                }
                else
                {
                    string lastId = lastInvoice.Id.Substring(0, lastInvoice.Id.IndexOf('-'));
                    var lastids = int.Parse(lastId) + 1;
                    invoices.Id = $"{lastids}-{date}";

                    _context.Add(invoices);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Report));

                }
              }
            return View(invoices);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices.FindAsync(id);
            if (invoices == null)
            {
                return NotFound();
            }
            return View(invoices);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Year,Date,From,To,PreviousConsumptionAmount,CurrentConsumptionAmount,ActualConsumptionAmount,ServiceFee,TaxRate,IsThereSanitation,ConsumptionValue,SanitationConsumptionValue,TotalInvoice,TaxValue,TotalBill,Notes")] Invoices invoices)
        {
            if (id != invoices.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoicesExists(invoices.Id))
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
            return View(invoices);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoices == null)
            {
                return NotFound();
            }

            return View(invoices);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoices = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoices);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoicesExists(string id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult GetSubscriptionInfoBySubscriptionId(string SubsId)
        {

            var model = Subscription.Get(a => a.Id == SubsId);
            return Json(model);
        }
        public JsonResult GetSubscriberInfoBySubscriberId(int SubId)
        {

            var model = subscriber.Get(a => a.Id == SubId);
            return Json(model);
        }
         public JsonResult GetInvoiceInfoByInvoiceId(string Id)
        {

            var model = Invoice.Get(a => a.Id == Id);
            return Json(model);
        }
    }
}

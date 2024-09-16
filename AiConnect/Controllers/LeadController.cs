using AiConnect.Models;
using AiConnect.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AiConnect.Controllers
{
    public class LeadController : Controller
    {
        private readonly OracleDbContext _contexto;

        public LeadController(OracleDbContext contexto)
        {
            _contexto = contexto;
        }

        // GET: Leads
        public async Task<IActionResult> Index()
        {
            var leads = await _contexto.Leads
                                       .Include(l => l.Cliente)
                                       .ToListAsync();
            return View(leads);
        }

        // GET: Leads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _contexto.Leads
                                       .Include(l => l.Cliente)
                                       .FirstOrDefaultAsync(l => l.Id == id);

            if (lead == null)
            {
                return NotFound();
            }

            return View(lead);
        }

        // GET: Lead/Create
        public IActionResult Create()
        {
            ViewBag.ClienteIds = new SelectList(_contexto.Clientes, "Id", "Nome");
            return View();
        }

        // POST: Lead/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Telefone,Email,Cargo,Empresa,ClienteId")] Lead lead)
        {
            if (ModelState.IsValid)
            {
                _contexto.Add(lead);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ClienteIds = new SelectList(_contexto.Clientes, "Id", "Nome", lead.ClienteId);
            return View(lead);
        }

        // GET: Leads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _contexto.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }

            ViewBag.ClienteIds = new SelectList(_contexto.Clientes, "Id", "Nome", lead.ClienteId);
            return View(lead);
        }

        // POST: Leads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Telefone,Email,Cargo,Empresa,ClienteId")] Lead lead)
        {
            if (id != lead.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contexto.Update(lead);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeadExists(lead.Id))
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
            ViewBag.ClienteIds = new SelectList(_contexto.Clientes, "Id", "Nome", lead.ClienteId);
            return View(lead);
        }

        // GET: Leads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _contexto.Leads
                .Include(l => l.Cliente)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lead == null)
            {
                return NotFound();
            }

            return View(lead);
        }

        // POST: Leads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lead = await _contexto.Leads.FindAsync(id);
            _contexto.Leads.Remove(lead);
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeadExists(int id)
        {
            return _contexto.Leads.Any(e => e.Id == id);
        }
    }
}



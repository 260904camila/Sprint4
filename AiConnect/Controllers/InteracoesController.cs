using AiConnect.Models;
using AiConnect.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace AiConnect.Controllers
{
    public class InteracoesController : Controller
    {
        private readonly OracleDbContext _contexto;

        public InteracoesController(OracleDbContext contexto)
        {
            _contexto = contexto;
        }

        // GET: Interacoes
        public async Task<IActionResult> Index()
        {
            var interacoes = await _contexto.Interacoes
                                            .Include(i => i.Cliente)
                                            .Include(i => i.Lead)
                                            .ToListAsync();
            return View(interacoes);
        }

        // GET: Interacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interacao = await _contexto.Interacoes
                .Include(i => i.Cliente)
                .Include(i => i.Lead)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (interacao == null)
            {
                return NotFound();
            }

            return View(interacao);
        }

        // GET: Interacoes/Create
        public IActionResult Create()
        {
            ViewBag.Clientes = new SelectList(_contexto.Clientes, "Id", "Nome");
            return View();
        }

        // POST: Interacoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataInteracao,Tipo,Descricao,ClienteId,LeadId")] Interacoes interacoes)
        {
            if (ModelState.IsValid)
            {
                _contexto.Add(interacoes);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clientes = new SelectList(_contexto.Clientes, "Id", "Nome", interacoes.ClienteId);
            return View(interacoes);
        }

        // GET: Interacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interacao = await _contexto.Interacoes.FindAsync(id);
            if (interacao == null)
            {
                return NotFound();
            }

            ViewBag.Clientes = new SelectList(_contexto.Clientes, "Id", "Nome", interacao.ClienteId);
            ViewBag.Leads = new SelectList(_contexto.Leads, "Id", "Nome", interacao.LeadId);

            return View(interacao);
        }


        // POST: Interacoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataInteracao,Tipo,Descricao,ClienteId,LeadId")] Interacoes interacao)
        {
            if (id != interacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contexto.Update(interacao);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_contexto.Interacoes.Any(i => i.Id == id))
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
            ViewBag.Clientes = new SelectList(_contexto.Clientes, "Id", "Nome", interacao.ClienteId);
            ViewBag.Leads = new SelectList(_contexto.Leads, "Id", "Nome", interacao.LeadId);
            return View(interacao);
        }

        // GET: Interacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interacao = await _contexto.Interacoes
                .Include(i => i.Cliente)
                .Include(i => i.Lead)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (interacao == null)
            {
                return NotFound();
            }

            return View(interacao);
        }

        // POST: Interacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interacao = await _contexto.Interacoes.FindAsync(id);
            _contexto.Interacoes.Remove(interacao);
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método para obter os leads com base no ID do cliente
        [HttpGet]
        public async Task<IActionResult> GetLeadsByClientId(int id)
        {
            var leads = await _contexto.Leads
                .Where(l => l.ClienteId == id)
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Nome })
                .ToListAsync();

            return Json(leads);
        }
    }
}


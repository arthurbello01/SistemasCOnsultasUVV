using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaConsultasUVV.Data;
using SistemaConsultasUVV.Models;

namespace SistemaConsultasUVV.Controllers
{
    [Authorize] // Só usuários autenticados acessam qualquer ação deste controller
    public class ConsultasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultasController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int UsuarioLogadoId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /Consultas
        public async Task<IActionResult> Index()
        {
            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == UsuarioLogadoId)
                .OrderBy(c => c.DataHora)
                .ToListAsync();

            return View(consultas);
        }

        // GET: /Consultas/Create
        public IActionResult Create() => View();

        // POST: /Consultas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consulta consulta)
        {
            ModelState.Remove(nameof(Consulta.UsuarioId));
            ModelState.Remove(nameof(Consulta.Usuario));

            if (!ModelState.IsValid)
                return View(consulta);

            consulta.UsuarioId = UsuarioLogadoId;

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Consulta cadastrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Consultas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioLogadoId);

            if (consulta == null) return NotFound();

            return View(consulta);
        }

        // POST: /Consultas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Consulta consultaEditada)
        {
            if (id != consultaEditada.Id) return NotFound();

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioLogadoId);

            if (consulta == null) return NotFound();

            ModelState.Remove(nameof(Consulta.UsuarioId));
            ModelState.Remove(nameof(Consulta.Usuario));

            if (!ModelState.IsValid)
                return View(consultaEditada);

            consulta.Especialidade = consultaEditada.Especialidade;
            consulta.DataHora = consultaEditada.DataHora;
            consulta.Descricao = consultaEditada.Descricao;

            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Consulta atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Consultas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioLogadoId);

            if (consulta == null) return NotFound();

            return View(consulta);
        }

        // POST: /Consultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioLogadoId);

            if (consulta == null) return NotFound();

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Consulta removida com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}

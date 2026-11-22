using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public PrestamosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bibliotecaDbContext = _context.Prestamos.Include(p => p.Libro).Include(p => p.Socio);
            return View(await bibliotecaDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Socio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        public IActionResult Create()
        {
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo");
            ViewData["SocioId"] = new SelectList(_context.Socios, "Id", "Nombre_Completo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha_Prestamo,Fecha_Devolucion,LibroId,SocioId")] Prestamo prestamo)
        {
            await ValidarPrestamo(prestamo);
            if (ModelState.IsValid)
            {
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewData["SocioId"] = new SelectList(_context.Socios, "Id", "Nombre_Completo", prestamo.SocioId);
            return View(prestamo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewData["SocioId"] = new SelectList(_context.Socios, "Id", "Nombre_Completo", prestamo.SocioId);
            return View(prestamo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha_Prestamo,Fecha_Devolucion,LibroId,SocioId")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            await ValidarPrestamo(prestamo);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id))
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
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewData["SocioId"] = new SelectList(_context.Socios, "Id", "Nombre_Completo", prestamo.SocioId);
            return View(prestamo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Socio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidarPrestamo(Prestamo prestamo)
        {
            if (prestamo.Fecha_Devolucion.HasValue && prestamo.Fecha_Devolucion.Value.Date < prestamo.Fecha_Prestamo.Date)
            {
                ModelState.AddModelError(nameof(Prestamo.Fecha_Devolucion),
                    "La fecha de devolución no puede ser anterior a la fecha de préstamo.");
                return;
            }

            var inicio = prestamo.Fecha_Prestamo.Date;
            var fin = prestamo.Fecha_Devolucion?.Date ?? DateTime.MaxValue;

            var cruce = await _context.Prestamos
                .Include(p => p.Socio)
                .Where(p => p.Id != prestamo.Id
                         && p.LibroId == prestamo.LibroId
                         && p.Fecha_Prestamo <= fin
                         && (p.Fecha_Devolucion == null || p.Fecha_Devolucion >= inicio))
                .OrderBy(p => p.Fecha_Prestamo)
                .FirstOrDefaultAsync();

            if (cruce != null)
            {
                var hasta = cruce.Fecha_Devolucion.HasValue
                    ? cruce.Fecha_Devolucion.Value.ToString("dd/MM/yyyy")
                    : "sin fecha de devolución";
                var quien = cruce.SocioId == prestamo.SocioId
                    ? "Este socio ya tiene"
                    : $"{cruce.Socio?.Nombre_Completo} ya tiene";

                ModelState.AddModelError(string.Empty,
                    $"{quien} este libro prestado del {cruce.Fecha_Prestamo:dd/MM/yyyy} al {hasta}. " +
                    "Elija otras fechas o registre primero la devolución.");
            }
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD.DataBase;
using CRUD.Models;
using CRUD.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace CRUD.Controllers
{
    [Authorize]
    public class VendedorController : Controller
    {
        private readonly ApplicationDBContext _context;

        public VendedorController(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NomeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nome_desc" : "";
            ViewData["EmailSortParm"] = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var vendedores = from s in _context.Vendedores
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                vendedores = vendedores.Where(s => s.Nome.Contains(searchString) || s.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "nome_desc":
                    vendedores = vendedores.OrderByDescending(s => s.Nome);
                    break;
                case "email_desc":
                    vendedores = vendedores.OrderByDescending(s => s.Email);
                    break;
                default:
                    vendedores = vendedores.OrderBy(s => s.Nome);
                    break;
            }

            int pageSize = 5;

            return View(await PaginatedList<Vendedor>.CreateAsync(vendedores.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Vendedores.Include("Clientes")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }

   
        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telefone")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendedor);
        }

     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Vendedores.FindAsync(id);
            if (vendedor == null)
            {
                return NotFound();
            }
            return View(vendedor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telefone")] Vendedor vendedor)
        {
            if (id != vendedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendedorExists(vendedor.Id))
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
            return View(vendedor);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Vendedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendedor = await _context.Vendedores.FindAsync(id);
            _context.Vendedores.Remove(vendedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendedorExists(int id)
        {
            return _context.Vendedores.Any(e => e.Id == id);
        }
    }
}

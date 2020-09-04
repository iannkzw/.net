using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD.DataBase;
using CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using CRUD.ViewModel;
using AutoMapper;

namespace CRUD.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public ClienteController(ApplicationDBContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NomeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nome_asc" : "nome_desc";
            ViewData["VendedorSortParm"] = String.IsNullOrEmpty(sortOrder) ? "vendedor_desc" : "";
            ViewData["TelefoneSortParm"] = sortOrder == "Tel" ? "tel_desc" : "Tel";
            ViewData["IDSortParm"] = sortOrder == "ID" ? "id_desc" : "ID";


            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var clientes = from s in _context.Clientes.Include(c => c.Vendedor)
                          
                           select s;



            if (!String.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Where(s => s.Nome.Contains(searchString) || s.Telefone.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ID":
                    clientes = clientes.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    clientes = clientes.OrderByDescending(s => s.Id);
                    break;
                case "nome_desc":
                    clientes = clientes.OrderByDescending(s => s.Nome);
                    break;
                 case "nome_asc":
                    clientes = clientes.OrderBy(s => s.Nome);
                    break;
                case "vendedor_desc":
                    clientes = clientes.OrderByDescending(s => s.Vendedor.Nome);
                    break;
                 case "Tel":
                    clientes = clientes.OrderBy(s => s.Telefone);
                    break;
                case "tel_desc":
                    clientes = clientes.OrderByDescending(s => s.Telefone);
                    break;
                default:
                    clientes = clientes.OrderByDescending(s => s.Id);
                    break;
            }

            int pageSize = 5;

            return View(await PaginatedList<Cliente>.CreateAsync(clientes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Vendedor)
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(m => m.Id == id);

            var c = _mapper.Map<Cliente, ClienteViewModel>(cliente);

            return View(c);
        }

       
        public IActionResult Create()
        {
            ViewData["VendedorID"] = new SelectList(_context.Vendedores, "Id", "Nome");
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel cliente)
        {
            if (ModelState.IsValid)
            {
                var c = _mapper.Map<ClienteViewModel, Cliente>(cliente);
                _context.Add(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VendedorID"] = new SelectList(_context.Vendedores, "Id", "Nome", cliente.VendedorID);
            return View(cliente);
        }

    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            var c = _mapper.Map<Cliente, ClienteViewModel>(cliente);
           
            ViewData["VendedorID"] = new SelectList(_context.Vendedores, "Id", "Nome", cliente.VendedorID);
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var c = _mapper.Map<ClienteViewModel, Cliente>(cliente);
                    _context.Update(c);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            ViewData["VendedorID"] = new SelectList(_context.Vendedores, "Id", "Nome", cliente.VendedorID);
            return View(cliente);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            var c = _mapper.Map<Cliente, ClienteViewModel>(cliente);

            return View(c);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}

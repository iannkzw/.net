using AutoMapper;
using CRUD.DataBase;
using CRUD.Models;
using CRUD.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public ProdutoController(ApplicationDBContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ClienteSortParm"] = String.IsNullOrEmpty(sortOrder) ? "cliente_asc" : "cliente_desc";
            ViewData["DescricaoSortParm"] = String.IsNullOrEmpty(sortOrder) ? "descricao_asc" : "descricao_desc";
            ViewData["ValorSortParm"] = sortOrder == "Valor" ? "valor_desc" : "Valor";
            ViewData["IDSortParm"] = sortOrder == "ID" ? "id_desc" : "ID";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";


            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var produtos = from s in _context.Produtos.Include(p => p.Cliente)
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                produtos = produtos.Where(s => s.Descricao.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ID":
                    produtos = produtos.OrderBy(s => s.ID);
                    break;
                case "id_desc":
                    produtos = produtos.OrderByDescending(s => s.ID);
                    break;
                case "descricao_desc":
                    produtos = produtos.OrderByDescending(s => s.Descricao);
                    break;
                case "descricao_asc":
                    produtos = produtos.OrderBy(s => s.Descricao);
                    break;
                case "cliente_desc":
                    produtos = produtos.OrderByDescending(s => s.Cliente.Nome);
                    break;
                case "cliente_asc":
                    produtos = produtos.OrderBy(s => s.Cliente.Nome);
                    break;
                case "Valor":
                    produtos = produtos.OrderBy(s => s.Valor);
                    break;
                case "valor_desc":
                    produtos = produtos.OrderByDescending(s => s.Valor);
                    break;
                case "Date":
                    produtos = produtos.OrderBy(s => s.CriadoEm);
                    break;
                case "date_desc":
                    produtos = produtos.OrderByDescending(s => s.CriadoEm);
                    break;
                default:
                    produtos = produtos.OrderByDescending(s => s.ID);
                    break;
            }

            int pageSize = 5;

            return View(await PaginatedList<Produto>.CreateAsync(produtos.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.ID == id);
            var p = _mapper.Map<Produto, ProdutoViewModel>(produto);

            if (produto == null)
            {
                return NotFound();
            }

            return View(p);
        }


        public IActionResult Create()
        {
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nome");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produto)
        {
            if (ModelState.IsValid)
            {
                var p = _mapper.Map<ProdutoViewModel, Produto>(produto); 

                p.CriadoEm = DateTime.Now;
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nome", produto.ClienteID);
            return View(produto);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            var p = _mapper.Map<Produto, ProdutoViewModel>(produto);

            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nome", produto.ClienteID);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProdutoViewModel produto)
        {
            if (id != produto.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var p = _mapper.Map<ProdutoViewModel, Produto>(produto);
                    _context.Update(p);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.ID))
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
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nome", produto.ClienteID);
            return View(produto);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.ID == id);

            var p = _mapper.Map<Produto, ProdutoViewModel>(produto);
          
            return View(p);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.ID == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUD.Models;
using CRUD.DataBase;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public HomeController(ApplicationDBContext context)
        {
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

            ViewBag.Soma = _context.Produtos.OrderBy(c => c.Valor).Sum(c => c.Valor).ToString("C", CultureInfo.CurrentCulture);
            ViewBag.TotalProdutos = _context.Produtos.Count();
            ViewBag.TotalClientes = _context.Clientes.Count();
            ViewBag.TotalVendedores = _context.Vendedores.Count();

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

            int pageSize = 4;

            return View(await PaginatedList<Produto>.CreateAsync(produtos.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
       
        
    }
}

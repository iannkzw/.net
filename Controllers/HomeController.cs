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

        public async Task<IActionResult> Index()
        {
            ViewBag.Soma = _context.Produtos.OrderBy(c => c.Valor).Take(2).Sum(c => c.Valor).ToString("C", CultureInfo.CurrentCulture);
            ViewBag.TotalProdutos = _context.Produtos.Count();

            return View();
        }
        
    }
}

using CRUD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.ViewModel
{
    public class ClienteViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int Telefone { get; set; }

        [Required]
        public string Produto { get; set; }

        [Required]
        public float Valor { get; set; }

        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}

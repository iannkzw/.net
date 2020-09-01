using CRUD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.ViewModel
{
    public class VendedorViewModel
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Campo Obrigatorio")]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "teste")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatorio")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Teste")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatorio")]
        public int Telefone { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}

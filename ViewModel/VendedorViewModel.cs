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
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Mínimo de caracteres: 3")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [StringLength(100, MinimumLength = 7, ErrorMessage = "Mínimo de caracteres: 7")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string Telefone { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}

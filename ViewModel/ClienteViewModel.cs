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
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Mínimo de caracteres: 3")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [MaxLength(15)]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string Cpf { get; set; }
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}

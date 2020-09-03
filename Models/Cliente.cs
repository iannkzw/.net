using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Models
{
    public class Cliente
    {
       public int Id { get; set; }  
        public string Nome{ get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public float Valor {get; set;}
        public string Produto { get; set; }
        public int VendedorID{ get; set; }
        public virtual Vendedor Vendedor{ get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}

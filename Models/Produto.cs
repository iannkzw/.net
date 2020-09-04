using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Models
{
    public class Produto
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public float Valor { get; set; }
        public DateTime CriadoEm { get; set; }
        public int ClienteID { get; set; }
        public virtual Cliente Cliente { get; set; }

    }
}

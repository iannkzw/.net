using CRUD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.ViewModel
{
    public class ProdutoViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "campo obrigatorio")]
        [MaxLength(150)]
        public string Descricao { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "campo obrigatorio")]
        public float Valor { get; set; }
        [Required]
        public DateTime CriadoEm { get; set; }
        public int ClienteID { get; set; }
        public virtual Cliente Cliente { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Infrastructura.Modelos
{
    public class IndiceInflacion
    {
        [Key]
        public int Id { get; set; }
        [StringLength(6)]
        public string Periodo { get; set; } // Formato "yyyymm"
        [Column(TypeName = "decimal(5,2)")]
        public decimal Valor { get; set; }
    }
}

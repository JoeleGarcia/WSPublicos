using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Infrastructura.Modelos
{
    public class TasaCambiaria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(3)]
        public string CodigoMoneda { get; set; } // "PES", "DOL", "EUR"
        [Column(TypeName = "decimal(5,2)")]
        public decimal Monto { get; set; }
    }
}

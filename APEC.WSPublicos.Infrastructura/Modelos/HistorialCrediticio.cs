using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Infrastructura.Modelos
{
    public class HistorialCrediticio
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string RncEmpresa { get; set; }
        public string ConceptoDeuda { get; set; }
        public DateTime Fecha { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoAdeudado { get; set; }
        [ForeignKey("Cliente")]
        [StringLength(20)]
        public string ClienteCedulaRnc { get; set; } // FK
        public Cliente Cliente { get; set; }
    }
}

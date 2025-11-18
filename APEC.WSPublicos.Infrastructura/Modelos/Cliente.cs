using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Infrastructura.Modelos
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CedulaRnc { get; set; } // PK
        public char IndicadorSalud { get; set; } // 'S'/'N'
        [Column(TypeName = "TEXT")]
        public string Comentario { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoTotalAdeudado { get; set; }

        public List<HistorialCrediticio> Historiales { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Infrastructura.Modelos
{
    public class RegistroUsoServicio
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string NombreServicio { get; set; } // Ej: "TasaCambio"
        public DateTime FechaInvocacion { get; set; }
    }
}

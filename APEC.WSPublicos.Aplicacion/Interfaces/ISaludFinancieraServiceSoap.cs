using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WSPublicos.Aplicacion.Interfaces
{
    [ServiceContract]
    public interface ISaludFinancieraServiceSoap
    {
        [OperationContract]
        SaludFinancieraResponse ConsultarSaludFinanciera(string cedulaRnc);
    }

    public class SaludFinancieraResponse
    {
        public string CedulaRnc { get; set; }
        public char IndicadorSalud { get; set; } // 'S' = Saludable, 'N' = No saludable
        public decimal MontoTotalAdeudado { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaConsulta { get; set; }
    }
}

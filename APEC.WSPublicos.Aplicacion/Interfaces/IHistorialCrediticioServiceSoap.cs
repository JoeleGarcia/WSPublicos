using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WSPublicos.Aplicacion.Interfaces
{
    [ServiceContract]
    public interface IHistorialCrediticioServiceSoap
    {
        [OperationContract]
        HistorialCrediticioResponse[] ConsultarHistorial(string cedulaRnc);
    }

    public class HistorialCrediticioResponse
    {
        public int Id { get; set; }
        public string RncEmpresa { get; set; }
        public string ConceptoDeuda { get; set; }
        public DateTime Fecha { get; set; }
        public decimal MontoAdeudado { get; set; }
        public string ClienteCedulaRnc { get; set; }
    }
}

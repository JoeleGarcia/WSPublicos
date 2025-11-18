using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WSPublicos.Aplicacion.Interfaces
{
    [ServiceContract]
    public interface IReporteUsoServiceSoap
    {
        [OperationContract]
        RegistroUsoResponse[] ConsultarUsoServicios(string nombreServicio, DateTime? fechaInicio, DateTime? fechaFin);
    }

    public class RegistroUsoResponse
    {
        public int Id { get; set; }
        public string NombreServicio { get; set; }
        public DateTime FechaInvocacion { get; set; }
    }
}

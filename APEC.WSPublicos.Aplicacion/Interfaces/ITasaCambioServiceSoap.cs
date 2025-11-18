using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Aplicacion.Interfaces
{
    [ServiceContract]
    public interface ITasaCambioServiceSoap
    {
        [OperationContract]
        decimal ObtenerTasa(string codigoMoneda);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WSPublicos.Aplicacion.Interfaces
{
    [ServiceContract]
    public interface IInflacionServiceSoap
    {
        [OperationContract]
        decimal ObtenerIndiceInflacion(string periodo);
    }
}

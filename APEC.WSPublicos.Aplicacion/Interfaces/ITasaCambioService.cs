using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Aplicacion.Interfaces
{
    public interface ITasaCambioService
    {
        decimal ObtenerTasa(string codigoMoneda);
    }
}

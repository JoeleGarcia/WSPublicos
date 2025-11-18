using APEC.WS.Infrastructura.Data;
using APEC.WSPublicos.Aplicacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WSPublicos.Aplicacion.Servicios
{
    public class InflacionServiceSoap : IInflacionServiceSoap
    {
        private readonly AppDbContext _context;

        public InflacionServiceSoap(AppDbContext context)
        {
            _context = context;
        }

        public decimal ObtenerIndiceInflacion(string periodo)
        {
            if (string.IsNullOrWhiteSpace(periodo))
                throw new FaultException("El periodo es obligatorio");

            periodo = periodo.Trim();

            if (periodo.Length != 6 || !int.TryParse(periodo, out _))
                throw new FaultException("Formato de período invalido. Debe ser 'yyyymm'");

            var currentYearMonth = DateTime.UtcNow.ToString("yyyyMM");
            if (string.Compare(periodo, currentYearMonth) > 0)
                throw new FaultException("No se puede consultar periodos futuros");

            var indice = _context.IndicesInflacion
                .AsNoTracking()
                .FirstOrDefault(i => i.Periodo == periodo);

            if (indice == null)
                throw new FaultException($"No se encontro indice para el periodo {periodo}");

            return indice.Valor;
        }
    }
}

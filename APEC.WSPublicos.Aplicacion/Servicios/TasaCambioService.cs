using APEC.WS.Aplicacion.Interfaces;
using APEC.WS.Infrastructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Aplicacion.Servicios
{
    public class TasaCambioService : ITasaCambioService
    {
        private readonly AppDbContext _context;

        public TasaCambioService(AppDbContext context)
        {
            _context = context;
        }

        public decimal ObtenerTasa(string codigoMoneda)
        {
            if (string.IsNullOrWhiteSpace(codigoMoneda))
            {
                throw new ArgumentException("El código de moneda no puede estar vacío", nameof(codigoMoneda));
            }

            codigoMoneda = codigoMoneda.Trim().ToUpper();

            var tasa = _context.TasasCambiarias
                .AsNoTracking()
                .FirstOrDefault(t => t.CodigoMoneda == codigoMoneda);

            if (tasa == null)
            {
                //_logger.LogWarning("Tasa no encontrada para moneda: {CodigoMoneda}", codigoMoneda);
                return 0m;
            }

            return tasa.Monto;
        }
    }
}

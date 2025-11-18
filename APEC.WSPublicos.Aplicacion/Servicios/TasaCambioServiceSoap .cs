using APEC.WS.Aplicacion.Interfaces;
using APEC.WS.Infrastructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APEC.WS.Aplicacion.Servicios
{
    public class TasaCambioServiceSoap : ITasaCambioServiceSoap
    {
        private readonly AppDbContext _context;

        public TasaCambioServiceSoap(AppDbContext context)
        {
            _context = context;
        }

        public decimal ObtenerTasa(string codigoMoneda)
        {
            if (string.IsNullOrWhiteSpace(codigoMoneda))
                throw new FaultException("El codigo de moneda es obligatorio");

            codigoMoneda = codigoMoneda.Trim().ToUpper();

            if (codigoMoneda.Length != 3)
                throw new FaultException("El codigo de moneda debe tener exactamente 3 caracteres");

            if (!Regex.IsMatch(codigoMoneda, @"^[A-Z]+$"))
                throw new FaultException("El codigo de moneda solo puede contener letras mayusculas");

            var tasa = _context.TasasCambiarias
                .AsNoTracking()
                .FirstOrDefault(t => t.CodigoMoneda == codigoMoneda);

            if (tasa == null)
                throw new FaultException($"No se encontro tasa para la moneda {codigoMoneda}");

            return tasa.Monto;
        }
    }
}

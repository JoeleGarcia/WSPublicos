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
    public class HistorialCrediticioServiceSoap : IHistorialCrediticioServiceSoap
    {
        private readonly AppDbContext _context;

        public HistorialCrediticioServiceSoap(AppDbContext context)
        {
            _context = context;
        }

        public HistorialCrediticioResponse[] ConsultarHistorial(string cedulaRnc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cedulaRnc))
                    throw new FaultException("La cedula/RNC es obligatoria");

                cedulaRnc = cedulaRnc.Trim();

                var historiales = _context.HistorialesCrediticios
                    .AsNoTracking()
                    .Where(h => h.ClienteCedulaRnc == cedulaRnc)
                    .OrderByDescending(h => h.Fecha)
                    .ToList();

                if (!historiales.Any())
                    throw new FaultException($"No se encontro historial crediticio para {cedulaRnc}");

                return historiales.Select(h => new HistorialCrediticioResponse
                {
                    Id = h.Id,
                    RncEmpresa = h.RncEmpresa,
                    ConceptoDeuda = h.ConceptoDeuda,
                    Fecha = h.Fecha,
                    MontoAdeudado = h.MontoAdeudado,
                    ClienteCedulaRnc = h.ClienteCedulaRnc
                }).ToArray();
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error interno: {ex.Message}");
            }
        }
    }
}

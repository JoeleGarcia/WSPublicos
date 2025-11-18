using APEC.WS.Infrastructura.Data;
using APEC.WS.Infrastructura.Modelos;
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
    public class ReporteUsoServiceSoap : IReporteUsoServiceSoap
    {
        private readonly AppDbContext _context;

        public ReporteUsoServiceSoap(AppDbContext context)
        {
            _context = context;
        }

        public RegistroUsoResponse[] ConsultarUsoServicios(string nombreServicio, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                IQueryable<RegistroUsoServicio> query = _context.RegistrosUso.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(nombreServicio))
                {
                    query = query.Where(r => r.NombreServicio.Contains(nombreServicio.Trim()));
                }

                if (fechaInicio.HasValue)
                {
                    query = query.Where(r => r.FechaInvocacion >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    // Incluir todo el día final
                    DateTime endOfDay = fechaFin.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(r => r.FechaInvocacion <= endOfDay);
                }

                // Ordenar por fecha descendente
                query = query.OrderByDescending(r => r.FechaInvocacion);

                var registros = query.ToList();

                return registros.Select(r => new RegistroUsoResponse
                {
                    Id = r.Id,
                    NombreServicio = r.NombreServicio,
                    FechaInvocacion = r.FechaInvocacion
                }).ToArray();
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error al consultar registros: {ex.Message}");
            }
        }
    }
}

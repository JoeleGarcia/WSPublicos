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
    public class SaludFinancieraServiceSoap : ISaludFinancieraServiceSoap
    {
        private readonly AppDbContext _context;

        public SaludFinancieraServiceSoap(AppDbContext context)
        {
            _context = context;
        }

        public SaludFinancieraResponse ConsultarSaludFinanciera(string cedulaRnc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cedulaRnc))
                    throw new FaultException("La cedula/RNC es obligatoria");

                cedulaRnc = cedulaRnc.Trim();

                var cliente = _context.Clientes
                    .Include(c => c.Historiales)
                    .FirstOrDefault(c => c.CedulaRnc == cedulaRnc);

                if (cliente == null)
                    throw new FaultException($"Cliente con cedula/RNC {cedulaRnc} no encontrado");

                var historial = new HistorialCrediticio
                {
                    RncEmpresa = "APEC",
                    ConceptoDeuda = "Consulta de Salud Financiera",
                    Fecha = DateTime.UtcNow,
                    MontoAdeudado = 0,
                    ClienteCedulaRnc = cliente.CedulaRnc
                };

                _context.HistorialesCrediticios.Add(historial);
                _context.SaveChanges();

                cliente.MontoTotalAdeudado = cliente.Historiales
                    .Where(h => h.MontoAdeudado > 0)
                    .Sum(h => h.MontoAdeudado);

                cliente.IndicadorSalud = cliente.MontoTotalAdeudado <= 50000 ? 'S' : 'N';
                _context.SaveChanges();

                return new SaludFinancieraResponse
                {
                    CedulaRnc = cliente.CedulaRnc,
                    IndicadorSalud = cliente.IndicadorSalud,
                    MontoTotalAdeudado = cliente.MontoTotalAdeudado,
                    Comentario = cliente.Comentario,
                    FechaConsulta = DateTime.UtcNow
                };
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

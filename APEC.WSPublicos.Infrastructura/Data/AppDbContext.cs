using APEC.WS.Infrastructura.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEC.WS.Infrastructura.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TasaCambiaria> TasasCambiarias { get; set; }
        public DbSet<IndiceInflacion> IndicesInflacion { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<HistorialCrediticio> HistorialesCrediticios { get; set; }
        public DbSet<RegistroUsoServicio> RegistrosUso { get; set; }
    }
}

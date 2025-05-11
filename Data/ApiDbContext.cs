using IoTSolution.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace IoTSolution.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<LeiturasModel> Leituras { get; set; }
        public DbSet<DispositivosModel> Dispositivos { get; set; }
        public DbSet<SensorsModel> Sensors { get; set; }
        public DbSet<UsuariosModel> Usuarios { get; set; }
        public DbSet<AlertasModel> Alertas { get; set; }
        public DbSet<LogsModel> Logs { get; set; }
    }
}

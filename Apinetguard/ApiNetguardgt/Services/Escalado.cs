using ApiNetguardgt.Data;
using ApiNetguardgt.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetguardgt.Services
{
    public class EscaladoService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EscaladoService> _logger;

        public EscaladoService(IServiceProvider serviceProvider, ILogger<EscaladoService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await EscalarIncidentes();
                // Revisa cada 5 minutos
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task EscalarIncidentes()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var limite = DateTime.UtcNow.AddHours(-2);

            var incidentesAEscalar = await context.Incidentes
                .Where(i =>
                    i.Estado == "Registrado" &&
                    (i.Severidad == "Critico" || i.Severidad == "Urgente") &&
                    i.FechaCreacion <= limite)
                .ToListAsync();

            foreach (var incidente in incidentesAEscalar)
            {
                var estadoAnterior = incidente.Estado;
                incidente.Estado = "Escalado";
                incidente.FechaActualizacion = DateTime.UtcNow;

                context.HistorialIncidentes.Add(new HistorialIncidente
                {
                    IncidenteId = incidente.Id,
                    EstadoAnterior = estadoAnterior,
                    EstadoNuevo = "Escalado",
                    Observacion = "Escalado automáticamente por superar 2 horas sin atención"
                });

                _logger.LogInformation($"Incidente {incidente.Id} escalado automáticamente.");
            }

            await context.SaveChangesAsync();
        }
    }
}
namespace ApiNetguardgt.Models
{
    public class HistorialIncidente
    {
        public int Id { get; set; }
        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;
        public DateTime FechaCambio { get; set; } = DateTime.UtcNow;
        public string? Observacion { get; set; }

        // Relaciones
        public int IncidenteId { get; set; }
        public Incidente? Incidente { get; set; }

        public int? TecnicoId { get; set; }
        public Tecnico? Tecnico { get; set; }
    }
}

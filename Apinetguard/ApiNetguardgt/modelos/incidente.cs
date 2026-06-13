namespace ApiNetguardgt.Models
{
    public class Incidente
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Severidad { get; set; } = string.Empty; 
        public string Estado { get; set; } = "Registrado"; 
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }

        // Relaciones
        public int SitioId { get; set; }
        public Sitio? Sitio { get; set; }

        public int? TecnicoId { get; set; }
        public Tecnico? Tecnico { get; set; }

        public ICollection<HistorialIncidente> Historial { get; set; } = new List<HistorialIncidente>();
    }
}

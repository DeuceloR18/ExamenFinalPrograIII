namespace ApiNetguardgt.DTOs
{
    public class IncidenteDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Severidad { get; set; } = string.Empty;
        public int SitioId { get; set; }
    }

    public class AsignarTecnicoDto
    {
        public int TecnicoId { get; set; }
    }

    public class CambiarEstadoDto
    {
        public string NuevoEstado { get; set; } = string.Empty;
        public string? Observacion { get; set; }
    }
}
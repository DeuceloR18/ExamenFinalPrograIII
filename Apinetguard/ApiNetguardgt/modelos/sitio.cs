namespace ApiNetguardgt.Models
{
    public class Sitio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // antena, nodo, POP
        public string Ubicacion { get; set; } = string.Empty;

        public ICollection<Incidente> Incidentes { get; set; } = new List<Incidente>();
    }
}
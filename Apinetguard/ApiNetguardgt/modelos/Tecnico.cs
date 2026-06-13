namespace ApiNetguardgt.Models
{
    public class Tecnico
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty; // fibra optica, microondas, sistemas electricos
        public bool Activo { get; set; } = true;

        public ICollection<Incidente> Incidentes { get; set; } = new List<Incidente>();
    }
}
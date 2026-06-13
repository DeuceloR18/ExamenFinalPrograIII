using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiNetguardgt.Data;
using ApiNetguardgt.DTOs;
using ApiNetguardgt.Models;

namespace ApiNetguardgt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IncidentesController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/incidentes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var incidentes = await _context.Incidentes
                .Include(i => i.Sitio)
                .Include(i => i.Tecnico)
                .ToListAsync();
            return Ok(incidentes);
        }

        // POST api/incidentes
        [HttpPost]
        public async Task<IActionResult> Create(IncidenteDto dto)
        {
            var incidente = new Incidente
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Severidad = dto.Severidad,
                SitioId = dto.SitioId,
                Estado = "Registrado"
            };
            _context.Incidentes.Add(incidente);
            await _context.SaveChangesAsync();
            return Ok(incidente);
        }

        // PUT api/incidentes/{id}/asignar
        [HttpPut("{id}/asignar")]
        public async Task<IActionResult> Asignar(int id, AsignarTecnicoDto dto)
        {
            var incidente = await _context.Incidentes.FindAsync(id);
            if (incidente == null) return NotFound("Incidente no encontrado");

            // Validar que el tecnico existe
            var tecnico = await _context.Tecnicos.FindAsync(dto.TecnicoId);
            if (tecnico == null) return NotFound("Tecnico no encontrado");

            // Validar max 3 incidentes activos
            var activos = await _context.Incidentes
                .CountAsync(i => i.TecnicoId == dto.TecnicoId && i.Estado != "Cerrado");
            if (activos >= 3)
                return BadRequest("El técnico ya tiene 3 incidentes activos");

            // Validar especialidad
            if (incidente.Severidad == "Critico" && tecnico.Especialidad != "fibra optica")
                return BadRequest("Solo técnicos de fibra óptica pueden atender incidentes críticos");

            var estadoAnterior = incidente.Estado;
            incidente.TecnicoId = dto.TecnicoId;
            incidente.Estado = "Asignado";
            incidente.FechaActualizacion = DateTime.UtcNow;

            // Historial
            _context.HistorialIncidentes.Add(new HistorialIncidente
            {
                IncidenteId = id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = "Asignado",
                TecnicoId = dto.TecnicoId
            });

            await _context.SaveChangesAsync();
            return Ok(incidente);
        }

        // PUT api/incidentes/{id}/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, CambiarEstadoDto dto)
        {
            var incidente = await _context.Incidentes.FindAsync(id);
            if (incidente == null) return NotFound("Incidente no encontrado");

            // Validar flujo de estados
            var flujo = new Dictionary<string, string>
            {
                { "Asignado", "EnProgreso" },
                { "EnProgreso", "Resuelto" },
                { "Resuelto", "Cerrado" }
            };

            if (!flujo.ContainsKey(incidente.Estado) || flujo[incidente.Estado] != dto.NuevoEstado)
                return BadRequest($"No se puede cambiar de {incidente.Estado} a {dto.NuevoEstado}");

            var estadoAnterior = incidente.Estado;
            incidente.Estado = dto.NuevoEstado;
            incidente.FechaActualizacion = DateTime.UtcNow;

            _context.HistorialIncidentes.Add(new HistorialIncidente
            {
                IncidenteId = id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = dto.NuevoEstado,
                TecnicoId = incidente.TecnicoId,
                Observacion = dto.Observacion
            });

            await _context.SaveChangesAsync();
            return Ok(incidente);
        }

        // GET api/incidentes/{id}/historial
        [HttpGet("{id}/historial")]
        public async Task<IActionResult> GetHistorial(int id)
        {
            var historial = await _context.HistorialIncidentes
                .Where(h => h.IncidenteId == id)
                .OrderBy(h => h.FechaCambio)
                .ToListAsync();
            return Ok(historial);
        }

        // GET api/incidentes/escalados
        [HttpGet("escalados")]
        public async Task<IActionResult> GetEscalados()
        {
            var ahora = DateTime.UtcNow;
            var incidentes = await _context.Incidentes
                .Where(i => i.Estado == "Escalado")
                .ToListAsync();
            return Ok(incidentes);
        }

        // GET api/incidentes/reporte
        [HttpGet("reporte")]
        public async Task<IActionResult> Reporte([FromQuery] string? estado, [FromQuery] int? tecnicoId)
        {
            var query = _context.Incidentes
                .Include(i => i.Tecnico)
                .Include(i => i.Sitio)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(i => i.Estado == estado);

            if (tecnicoId.HasValue)
                query = query.Where(i => i.TecnicoId == tecnicoId);

            var resultado = await query.ToListAsync();
            return Ok(resultado);
        }
    }
}
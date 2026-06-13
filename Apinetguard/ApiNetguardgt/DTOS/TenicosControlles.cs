using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiNetguardgt.Data;
using ApiNetguardgt.DTOs;
using ApiNetguardgt.Models;

namespace ApiNetguardgt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TecnicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TecnicosController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/tecnicos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tecnicos = await _context.Tecnicos.ToListAsync();
            return Ok(tecnicos);
        }

        // POST api/tecnicos
        [HttpPost]
        public async Task<IActionResult> Create(TecnicoDto dto)
        {
            var tecnico = new Tecnico
            {
                Nombre = dto.Nombre,
                Especialidad = dto.Especialidad
            };
            _context.Tecnicos.Add(tecnico);
            await _context.SaveChangesAsync();
            return Ok(tecnico);
        }

        // GET api/tecnicos/{id}/incidentes
        [HttpGet("{id}/incidentes")]
        public async Task<IActionResult> GetIncidentes(int id)
        {
            var incidentes = await _context.Incidentes
                .Where(i => i.TecnicoId == id && i.Estado != "Cerrado")
                .ToListAsync();
            return Ok(new { total = incidentes.Count, incidentes });
        }
    }
}
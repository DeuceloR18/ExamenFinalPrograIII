using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiNetguardgt.Data;
using ApiNetguardgt.DTOs;
using ApiNetguardgt.Models;

namespace ApiNetguardgt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SitiosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SitiosController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/sitios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sitios = await _context.Sitios.ToListAsync();
            return Ok(sitios);
        }

        // POST api/sitios
        [HttpPost]
        public async Task<IActionResult> Create(SitioDto dto)
        {
            var sitio = new Sitio
            {
                Nombre = dto.Nombre,
                Tipo = dto.Tipo,
                Ubicacion = dto.Ubicacion
            };
            _context.Sitios.Add(sitio);
            await _context.SaveChangesAsync();
            return Ok(sitio);
        }
    }
}
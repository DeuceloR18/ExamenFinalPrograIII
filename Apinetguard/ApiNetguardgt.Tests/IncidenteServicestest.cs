using ApiNetguardgt.Controllers;
using ApiNetguardgt.Data;
using ApiNetguardgt.DTOs;
using ApiNetguardgt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace ApiNetguardgt.Tests
{
    public class IncidenteServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        // TEST 1: Crear incidente correctamente
        [Fact]
        public async Task CrearIncidente_DebeRetornarOk()
        {
            var context = GetDbContext();
            var sitio = new Sitio { Nombre = "Sitio A", Tipo = "nodo", Ubicacion = "Guatemala" };
            context.Sitios.Add(sitio);
            await context.SaveChangesAsync();

            var controller = new IncidentesController(context);
            var dto = new IncidenteDto
            {
                Titulo = "Falla de red",
                Descripcion = "Sin señal",
                Severidad = "Critico",
                SitioId = sitio.Id
            };

            var result = await controller.Create(dto);

            Assert.IsType<OkObjectResult>(result);
        }

        // TEST 2: Incidente se crea con estado Registrado
        [Fact]
        public async Task CrearIncidente_EstadoDebeSerRegistrado()
        {
            var context = GetDbContext();
            var sitio = new Sitio { Nombre = "Sitio B", Tipo = "antena", Ubicacion = "Jalapa" };
            context.Sitios.Add(sitio);
            await context.SaveChangesAsync();

            var controller = new IncidentesController(context);
            var dto = new IncidenteDto
            {
                Titulo = "Test",
                Descripcion = "Test desc",
                Severidad = "Moderado",
                SitioId = sitio.Id
            };

            var result = await controller.Create(dto) as OkObjectResult;
            var incidente = result?.Value as Incidente;

            Assert.Equal("Registrado", incidente?.Estado);
        }

        // TEST 3: Tecnico no puede tener mas de 3 incidentes activos
        [Fact]
        public async Task AsignarTecnico_ConMasDe3Activos_DebeRetornarBadRequest()
        {
            var context = GetDbContext();
            var tecnico = new Tecnico { Nombre = "Juan", Especialidad = "fibra optica" };
            var sitio = new Sitio { Nombre = "Sitio C", Tipo = "POP", Ubicacion = "Jalapa" };
            context.Tecnicos.Add(tecnico);
            context.Sitios.Add(sitio);
            await context.SaveChangesAsync();

            // Agregar 3 incidentes activos al tecnico
            for (int i = 0; i < 3; i++)
            {
                context.Incidentes.Add(new Incidente
                {
                    Titulo = $"Incidente {i}",
                    Descripcion = "desc",
                    Severidad = "Moderado",
                    Estado = "Asignado",
                    SitioId = sitio.Id,
                    TecnicoId = tecnico.Id
                });
            }
            await context.SaveChangesAsync();

            // Intentar asignar un 4to incidente
            var nuevoIncidente = new Incidente
            {
                Titulo = "Incidente 4",
                Descripcion = "desc",
                Severidad = "Moderado",
                Estado = "Registrado",
                SitioId = sitio.Id
            };
            context.Incidentes.Add(nuevoIncidente);
            await context.SaveChangesAsync();

            var controller = new IncidentesController(context);
            var result = await controller.Asignar(nuevoIncidente.Id, new AsignarTecnicoDto { TecnicoId = tecnico.Id });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // TEST 4: Cambio de estado debe seguir el flujo correcto
        [Fact]
        public async Task CambiarEstado_FlujoIncorrecto_DebeRetornarBadRequest()
        {
            var context = GetDbContext();
            var incidente = new Incidente
            {
                Titulo = "Test",
                Descripcion = "desc",
                Severidad = "Bajo",
                Estado = "Registrado",
                SitioId = 1
            };
            context.Incidentes.Add(incidente);
            await context.SaveChangesAsync();

            var controller = new IncidentesController(context);
            var result = await controller.CambiarEstado(incidente.Id, new CambiarEstadoDto
            {
                NuevoEstado = "Cerrado" // saltar estados
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // TEST 5: Listar incidentes retorna OK
        [Fact]
        public async Task GetAll_DebeRetornarOk()
        {
            var context = GetDbContext();
            var controller = new IncidentesController(context);

            var result = await controller.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        // TEST 6: Crear tecnico correctamente
        [Fact]
        public async Task CrearTecnico_DebeRetornarOk()
        {
            var context = GetDbContext();
            var controller = new TecnicosController(context);
            var dto = new TecnicoDto
            {
                Nombre = "Pedro",
                Especialidad = "microondas"
            };

            var result = await controller.Create(dto);

            Assert.IsType<OkObjectResult>(result);
        }

        // TEST 7: Crear sitio correctamente
        [Fact]
        public async Task CrearSitio_DebeRetornarOk()
        {
            var context = GetDbContext();
            var controller = new SitiosController(context);
            var dto = new SitioDto
            {
                Nombre = "Nodo Central",
                Tipo = "nodo",
                Ubicacion = "Ciudad de Guatemala"
            };

            var result = await controller.Create(dto);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
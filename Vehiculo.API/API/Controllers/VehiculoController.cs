using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/vehiculo")]
    [ApiController]
    [Authorize]
    public class VehiculoController : ControllerBase, IVehiculoController
    {
        private IVehiculoFlujo _vehiculoFlujo;
        private ILogger<VehiculoController> _logger;
        public VehiculoController(IVehiculoFlujo vehiculoFlujo, ILogger<VehiculoController> logger)
        {
            _vehiculoFlujo = vehiculoFlujo;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Agregar([FromBody]VehiculoRequest vehiculo)
        {
            var resultado = await _vehiculoFlujo.Agregar(vehiculo);
            return CreatedAtAction(nameof(ObtenerPorId), new {Id=resultado},null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Editar([FromRoute]Guid id,  [FromBody]VehiculoRequest vehiculo)
        {
            string mensajeError = "El vehiculo no existe";
            Boolean existeVehiculo = await VerificarExistenciaVehiculo(id);
            if (!existeVehiculo)
            {
                return BadRequest(mensajeError);
            }
            var vehiculoEditar = await _vehiculoFlujo.Editar(id,vehiculo);
            return Ok(vehiculoEditar);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Eliminar([FromRoute] Guid id)
        {
            Boolean existeVehiculo = await VerificarExistenciaVehiculo(id);
            if (!existeVehiculo)
            {
                return BadRequest("El vehiculo no existe");
            }
            var resultado = await _vehiculoFlujo.Eliminar(id);
           
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _vehiculoFlujo.Obtener();
            if (!resultado.Any())
            {
                return NoContent();
            }
            return Ok(resultado);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid id)
        {
            var resultado = await _vehiculoFlujo.ObtenerPorId(id);
            if (resultado == null || resultado.Id == Guid.Empty)
            {
                return NoContent();
            }
            return Ok(resultado);
        }

        private async Task<Boolean> VerificarExistenciaVehiculo(Guid id)
        {
             var vehiculo = await _vehiculoFlujo.ObtenerPorId(id);
            if(vehiculo == null)
            {
                return false;
            }
            return true;
        }
    }
}

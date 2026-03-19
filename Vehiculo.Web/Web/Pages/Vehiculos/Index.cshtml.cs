using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Abstracciones.Modelos.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Pages.Vehiculos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public IList<VehiculoResponse> vehiculos { get; set; } = default!;

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet()
        {
            using var cliente = ObtenerClienteConToken();
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerVehiculos");
            var solicitud = new HttpRequestMessage(HttpMethod.Get,endpoint);
            var respuesta = await cliente.SendAsync(solicitud);
            if (!respuesta.IsSuccessStatusCode)
            {
                // Pon un Punto de Interrupción (Breakpoint) aquí
                var contenidoError = await respuesta.Content.ReadAsStringAsync();
                var codigoStatus = respuesta.StatusCode;

                // Esto te dirá exactamente qué dice el servidor (ej: "Usuario no autorizado" o "ID no encontrado")
                throw new Exception($"Error de API ({codigoStatus}): {contenidoError}");
            }
            respuesta.EnsureSuccessStatusCode();
            var resultado=await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            vehiculos = JsonSerializer.Deserialize<List<VehiculoResponse>>(resultado, opciones);
        }



        private HttpClient ObtenerClienteConToken()
        {
            var tokenClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "AccessToken");
            var cliente = new HttpClient();
            if (tokenClaim != null)
                cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", tokenClaim.Value);
            return cliente;
        }
    }
}

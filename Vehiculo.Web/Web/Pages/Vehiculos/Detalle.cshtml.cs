using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Vehiculos
{
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public VehiculoResponse Vehiculo { get; set; } = default!;
        public DetalleModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet(Guid? id)
        {
            if (id == null)
            {
                RedirectToPage("./Index");
            }
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerVehiculo");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint,id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Vehiculo = JsonSerializer.Deserialize<VehiculoResponse>(resultado, opciones);
            if (Vehiculo==null)
            {
                RedirectToPage("./Index");
            }
        }
    }
}

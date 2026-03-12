using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Vehiculos
{
    public class AgregarModel : PageModel
    {
        private IConfiguracion _configuracion;

        [BindProperty]
        public VehiculoRequest vehiculo { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> marcas { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> modelos { get; set; } = default!;
        [BindProperty]
        public Guid marcaSeleccionada { get; set; } = default!;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<PageResult> OnGet()
        {
            await ObtenerMarcas();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarVehiculo");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Post, endpoint);
            var respuesta = await cliente.PostAsJsonAsync(endpoint,vehiculo);
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");
        }

        private async Task ObtenerMarcas()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerMarcas");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultadoDeSerializado = JsonSerializer.Deserialize<List<Marca>>(resultado, opciones);
            marcas = resultadoDeSerializado.Select(marca => new SelectListItem { 
                Value=marca.Id.ToString(),
                Text=marca.Nombre.ToString()
            }).ToList();
        }


        private async Task<List<Modelo>?> ObtenerModelos(Guid marcaID)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerModelos");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, marcaID));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<Modelo>>(resultado, opciones);
            }
            return new List<Modelo>();
        }

        public async Task<JsonResult> OnGetObtenerModelos(Guid marcaID)
        {
            var modelos = await ObtenerModelos(marcaID);
            return new JsonResult(modelos);
        }
    }
}

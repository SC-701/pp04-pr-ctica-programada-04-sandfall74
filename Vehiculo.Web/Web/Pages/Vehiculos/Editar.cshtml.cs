using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Vehiculos
{
    [Authorize]
    public class EditarModel : PageModel
    {
        private IConfiguracion _configuracion;
        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        [BindProperty]
        public VehiculoResponse vehiculoResponse { get; set; } = default!;
        [BindProperty]
        public VehiculoRequest vehiculoRequest { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> marcas { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> modelos { get; set; } = default!;
        [BindProperty]
        public Guid marcaSeleccionada { get; set; } = default!;
        [BindProperty]
        public Guid modeloSeleccionado { get; set; } = default!;



        
        public async Task<ActionResult> OnGet(Guid? id)
        {
            if (id == null)
                return NotFound();
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerVehiculo");
            using var cliente = ObtenerClienteConToken();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                await ObtenerMarcas();
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                vehiculoResponse = JsonSerializer.Deserialize<VehiculoResponse>(resultado, opciones);
                if (vehiculoResponse != null)
                {
                    marcaSeleccionada = Guid.Parse(marcas.Where(m => m.Text == vehiculoResponse.Marca).FirstOrDefault().Value);
                    modelos = (await ObtenerModelosAsync(marcaSeleccionada)).Select(a =>
                        new SelectListItem
                        {
                            Value = a.Id.ToString(),
                            Text = a.Nombre.ToString(),
                            Selected = a.Nombre == vehiculoResponse.Modelo
                        }).ToList();
                    modeloSeleccionado = Guid.Parse(modelos.Where(m => m.Text == vehiculoResponse.Modelo).FirstOrDefault().Value);
                }

            }
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (vehiculoResponse.Id == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarVehiculo");
            using var cliente = ObtenerClienteConToken();

            var respuesta = await cliente.PutAsJsonAsync<VehiculoRequest>(string.Format(endpoint, vehiculoResponse.Id.ToString()), new VehiculoRequest { 
                IdModelo = modeloSeleccionado, 
                Anio = vehiculoResponse.Anio, 
                Color = vehiculoResponse.Color,
                CorreoPropietario = vehiculoResponse.CorreoPropietario, 
                Placa = vehiculoResponse.Placa,
                Precio = vehiculoResponse.Precio, 
                TelefonoPropietario = vehiculoResponse.TelefonoPropietario });
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");
        }
        private async Task ObtenerMarcas()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerMarcas");
            using var cliente = ObtenerClienteConToken();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var resultadoDeserializado = JsonSerializer.Deserialize<List<Marca>>(resultado, opciones);
                marcas = resultadoDeserializado.Select(a =>
                    new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Nombre.ToString()
                    }).ToList();
            }
        }
        public async Task<JsonResult> OnGetObtenerModelos(Guid marcaID)
        {
            var modelos = await ObtenerModelosAsync(marcaID);
            return new JsonResult(modelos);
        }

        private async Task<List<Modelo>> ObtenerModelosAsync(Guid marcaID)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerModelos");
            using var cliente = ObtenerClienteConToken();
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
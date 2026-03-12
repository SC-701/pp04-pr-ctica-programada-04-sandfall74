using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios.Registro;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Servicios
{

    public class RegistroServicio : IRegistroServicio
    {
        private readonly IConfiguracion _IConfiguracion;
        private readonly IHttpClientFactory _httpClientFactory;

        public RegistroServicio(IConfiguracion iConfiguracion, IHttpClientFactory httpClientFactory)
        {
            _IConfiguracion = iConfiguracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Propietario> ObtenerPlaca(string Placa)
        {
            var endPoint = _IConfiguracion.ObtenerMetodo("ApiEndPointsRegistro", "ObtenerRegistro");
            var servicioRegistro = _httpClientFactory.CreateClient("ServicioRegistro");
            var respuesta = await servicioRegistro.GetAsync(string.Format(endPoint,Placa));
            respuesta.EnsureSuccessStatusCode();

            var resultado = await  respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var resultadoDeserializado =  JsonSerializer.Deserialize<List<Propietario>>(resultado, opciones);
            return resultadoDeserializado.FirstOrDefault();
        }
    }
}

using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios.Registro;
using Abstracciones.Modelos.Servicios.Revision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios
{
    public class RevisionServicio : IRevisionServicio
    {
        private readonly IConfiguracion _IConfiguracion;
        private readonly IHttpClientFactory _httpClientFactory;

       

        public RevisionServicio(IConfiguracion iConfiguracion, IHttpClientFactory httpClientFactory)
        {
            _IConfiguracion = iConfiguracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Revision> Obtener(string Placa)
        {
            var endPoint = _IConfiguracion.ObtenerMetodo("ApiEndPointsRevision", "ObtenerRevision");
            var servicioRevision = _httpClientFactory.CreateClient("ServicioRevision");
            var respuesta = await servicioRevision.GetAsync(string.Format(endPoint, Placa));
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var resultadoDeserializado = JsonSerializer.Deserialize<List<Revision>>(resultado, opciones);
            return resultadoDeserializado.FirstOrDefault();
        }
    }
}

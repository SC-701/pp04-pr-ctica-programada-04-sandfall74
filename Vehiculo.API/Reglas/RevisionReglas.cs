using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios.Revision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public class RevisionReglas : IRevisionReglas
    {
        private readonly IRevisionServicio _revisionServicio;
        private readonly IConfiguracion _configuracion;

        public RevisionReglas(IRevisionServicio revisionServicio, IConfiguracion configuracion)
        {
            _revisionServicio = revisionServicio;
            _configuracion = configuracion;
        }

        public async Task<bool> RevisionEsValida(string placa)
        {
           var resultadoRevision = await _revisionServicio.Obtener(placa);
            if(ValidaEstado(resultadoRevision)&&ValidarPeriodo(resultadoRevision.Periodo))
                return true;
            return false;
        }



        private bool ValidaEstado(Revision resultadoRevision)
        {
            string estadoRevison = _configuracion.ObtenerValor("EstadoRevisionSatisfactorio");
            return resultadoRevision.Resultado==estadoRevison;
         }
        private static string ObtenerPeriodoActual()
        {

            return $"{DateTime.Now.Month}-{DateTime.Now.Year}";
        }

        private static bool ValidarPeriodo(string periodo)
        {
            return periodo == ObtenerPeriodoActual();
        }
    }
}

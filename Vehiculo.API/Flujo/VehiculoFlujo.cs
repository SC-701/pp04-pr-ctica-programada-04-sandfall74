using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;

namespace Flujo
{
    public class VehiculoFLujo : IVehiculoFlujo
    {
        private readonly IVehiculoDA _VehiculoDA;
        private readonly IRegistroReglas _reglas;
        private readonly IRevisionReglas _revisionReglas;

        public VehiculoFLujo(IVehiculoDA vehiculoDA, IRegistroReglas reglas, IRevisionReglas revisionReglas)
        {
            //hola
            _VehiculoDA = vehiculoDA;
            _reglas = reglas;
            _revisionReglas = revisionReglas;
        }

        public async Task<Guid> Agregar(VehiculoRequest vehiculo)
        {
            return await _VehiculoDA.Agregar(vehiculo);
        }

        public async Task<Guid> Editar(Guid id, VehiculoRequest vehiculo)
        {
            return await _VehiculoDA.Editar(id, vehiculo);
        }

        public async Task<Guid> Eliminar(Guid id)
        {
            return await _VehiculoDA.Eliminar(id);
        }

        public async Task<IEnumerable<VehiculoResponse>> Obtener()
        {
           return await _VehiculoDA.Obtener();
        }

        public async Task<VehiculoDetalle> ObtenerPorId(Guid id)
        {
            var vehiculo =  await _VehiculoDA.ObtenerPorId(id);
            //vehiculo.RegistroValido = await _reglas.VehiculoEstaRegistrado(vehiculo.Placa
            //    , vehiculo.CorreoPropietario);
            //vehiculo.RevisionValida = await _revisionReglas.RevisionEsValida(vehiculo.Placa);

            return vehiculo;
        }
    }
}

using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IVehiculoFlujo
    {
        Task<IEnumerable<VehiculoResponse>> Obtener();
        Task<VehiculoDetalle> ObtenerPorId(Guid id);
        Task<Guid> Agregar(VehiculoRequest vehiculo);
        Task<Guid> Editar(Guid id, VehiculoRequest vehiculo);

        Task<Guid> Eliminar(Guid id);
    }
}

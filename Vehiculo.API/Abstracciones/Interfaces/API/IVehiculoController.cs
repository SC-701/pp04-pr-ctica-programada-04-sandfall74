using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.API
{
    public interface IVehiculoController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid id);
        Task<IActionResult> Agregar(VehiculoRequest vehiculo);
        Task<IActionResult> Editar(Guid id, VehiculoRequest vehiculo);

        Task<IActionResult> Eliminar(Guid id);
    }
}

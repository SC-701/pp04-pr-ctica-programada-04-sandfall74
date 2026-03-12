using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class VehiculoDA : IVehiculoDA
    {
        private IRepositorioDapper _repositoriDapper;
        private SqlConnection _sqlConnection;

        public VehiculoDA(IRepositorioDapper repositoriDapper)
        {
            _repositoriDapper = repositoriDapper;
            _sqlConnection = _repositoriDapper.ObtenerRepositorio();
        }
        public async Task<Guid> Agregar(VehiculoRequest vehiculo)
        {
            string query= @"AgregarVehiculo";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
               Id =Guid.NewGuid(),
               IdModelo= vehiculo.IdModelo,
                Placa= vehiculo.Placa,
                Color= vehiculo.Color,
                Anio= vehiculo.Anio
                ,
                Precio=vehiculo.Precio
                ,
                CorreoPropietario = vehiculo.CorreoPropietario,
                TelefonoPropietario = vehiculo.TelefonoPropietario

            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid id, VehiculoRequest vehiculo)
        {
            (bool flowControl, Guid value) = await VerificarVehiculoExiste(id);
            if (!flowControl)
            {
                return value;
            }
            string query = @"UpdateVehiculo";
            
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = id,
                IdModelo = vehiculo.IdModelo,
                Placa = vehiculo.Placa,
                Color = vehiculo.Color,
                Anio = vehiculo.Anio
                ,
                Precio = vehiculo.Precio
                ,
                CorreoPropietario = vehiculo.CorreoPropietario,
                TelefonoPropietario = vehiculo.TelefonoPropietario

            });
            return resultadoConsulta;
        }

        private async Task<(bool flowControl, Guid value)> VerificarVehiculoExiste(Guid id)
        {
            VehiculoResponse? vehiculoResponse = await ObtenerPorId(id);
            if (vehiculoResponse == null)
            {
                return (flowControl: false, value: Guid.Empty);
            }

            return (flowControl: true, value: default);
        }

        public async Task<Guid> Eliminar(Guid id)
        {
            (bool flowControl, Guid value) = await VerificarVehiculoExiste(id);
            if (!flowControl)
            {
                return value;
            }
            string query = @"EliminarVehiculo";

            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = id

            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<VehiculoResponse>> Obtener()
        {
            string query = @"ObtenerVehiculos";
            var resultadoConsulta =  await _sqlConnection.QueryAsync<VehiculoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<VehiculoDetalle> ObtenerPorId(Guid id)
        {
            string query = @"ObtenerVehiculo";
            var resultadoConsulta = await _sqlConnection.QueryAsync<VehiculoDetalle>(query, new {Id=id});
            return resultadoConsulta.FirstOrDefault();
        }
    }
}

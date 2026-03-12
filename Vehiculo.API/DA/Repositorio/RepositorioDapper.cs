using Abstracciones.Interfaces.DA;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.Repositorio
{
    public class RepositorioDapper : IRepositorioDapper
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _conexionBaseDatos;
        public RepositorioDapper(IConfiguration configuration)
        {
            _configuration = configuration;
            _conexionBaseDatos = new SqlConnection(_configuration.GetConnectionString("BD"));
        }

        SqlConnection IRepositorioDapper.ObtenerRepositorio()
        {
            return _conexionBaseDatos;
        }
    }
}

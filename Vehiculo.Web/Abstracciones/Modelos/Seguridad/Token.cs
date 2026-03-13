using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos.Seguridad
{
    public class Token
    {
        public bool ValidacionExitosa { get; set; }
        public string AccessToken { get; set; }
    }
}

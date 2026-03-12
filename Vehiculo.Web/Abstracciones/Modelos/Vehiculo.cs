using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
    public class VehiculoBase
    {

        [Required(ErrorMessage = "La propiedad placa es requerida")]
        [RegularExpression(@"[A-Za-z]{3}-[0-9]{3}", ErrorMessage = "El formato de la placa es inválido. El formato correcto es 'ABC-123'.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "La propiedad color es requerida")]
        [StringLength(40, ErrorMessage = "El color no puede tener más de 40 caracteres.", MinimumLength =4)]
        public string Color { get; set; }

        [Required(ErrorMessage = "La propiedad Anio es requerida")]
        [RegularExpression(@"(19|20)\d\d", ErrorMessage ="El formato del anio no es correcto, debe calcar con 20 o 19 seguido de dos numeros")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "La propiedad precio es requerida")]
        public Decimal Precio { get; set; }
        [Required(ErrorMessage = "La propiedad Correo Propietario es requerida")]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        public String CorreoPropietario { get; set; }
        [Required(ErrorMessage = "La propiedad Telefono Propietario es requerida")]
        [Phone(ErrorMessage = "El formato del teléfono es inválido")]
        public String TelefonoPropietario { get; set; }
    }

    public class VehiculoRequest : VehiculoBase
    {
        public Guid IdModelo { get; set; }
    }
    public class VehiculoResponse : VehiculoBase
    {
        public Guid Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
    }

    public class VehiculoDetalle: VehiculoResponse
    {
        public bool RevisionValida { get; set; }

        public bool RegistroValido { get; set; }

    }
}

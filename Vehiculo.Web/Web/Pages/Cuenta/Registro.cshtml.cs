// Registro.cshtml.cs
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Seguridad;
using Autorizacion.Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using Usuario = Abstracciones.Modelos.Seguridad.Usuario;

namespace Web.Pages.Cuenta
{
    public class RegistroModel : PageModel
    {
        [BindProperty]
        public Usuario usuario { get; set; } = default!;
        private IConfiguracion _configuracion;

        public RegistroModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var hash = Autenticacion.GenerarHash(usuario.Password);
            usuario.PasswordHash = Autenticacion.ObtenerHash(hash);

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "Registro");
            var cliente = new HttpClient();
            var respuesta = await cliente.PostAsJsonAsync<UsuarioBase>(endpoint, usuario);
            return RedirectToPage("../index");
        }
    }
}
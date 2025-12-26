using appWeb_Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace appWeb_Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if (!string.IsNullOrEmpty(usuario))
            {
                // Si ya hay sesión, ir al Home
                return RedirectToAction("Index", "Home");
            }

            return View(); // sino, muestra login
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Credenciales hardcodeadas
            var usuarioCorrecto = "Admin123";
            var passwordCorrecta = "Admin123";

            if (model.Usuario == usuarioCorrecto && model.Contrasena == passwordCorrecta)
            {
                HttpContext.Session.SetString("Usuario", model.Usuario);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.MensajeError = "Usuario o contraseña incorrectos";
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Usuario");
            return RedirectToAction("Login");
        }
    }
}

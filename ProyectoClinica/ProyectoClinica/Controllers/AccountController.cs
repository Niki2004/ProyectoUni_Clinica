using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Owin.Security;
using ProyectoClinica.Models;


namespace ProyectoClinica.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _context;
        private static Random _random = new Random();

        public AccountController()
        {
            _context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
      
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Primero buscamos el usuario por email
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                // Intentamos iniciar sesión con el email como nombre de usuario
                var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
                
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToAction("Redirect", "Redirect", new { email = model.Email });
                    #region Codigo para sprint#4
                    //// Verificamos el rol del usuario
                    //var roles = await UserManager.GetRolesAsync(user.Id);
                    //if (roles.Contains("Administrador"))
                    //{
                    //    return RedirectToAction("Index", "Administrativos");
                    //}
                    //else if (roles.Contains("Usuario"))
                    //{
                    //    return RedirectToAction("VistaCita", "Cita");
                    //}
                    //else
                    //{
                    //    return RedirectToAction("Index", "Home");
                    //}
                    #endregion
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Intento de inicio de sesión no válido");
                        return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "No se encontró un usuario con ese correo electrónico.");
                return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
           
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

       
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código inválido");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Nombre + " " + model.Apellido, Email = model.Email, Nombre = model.Nombre, 
                    Apellido = model.Apellido, Edad_Paciente = model.Edad_Paciente, Genero_Paciente = model.Genero_Paciente.ToString(),
                    Direccion = model.Direccion, Cedula = model.Cedula, Imagen = model.Imagen, PhoneNumber = model.PhoneNumber};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Usuario");
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    return RedirectToAction("VistaCita", "Cita");
                }
                AddErrors(result);
            }

        
            return View(model);
        }

   
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

 
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Metodo de Olvidar Contrasena
        /// </summary>
        /// <returns></returns>
        //Metodo de Olvido Contrasena
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                if (_context.Recuperacion_Contra.Any(r => r.Id == user.Id)) 
                {
                    ModelState.AddModelError("", "Ya se ha enviado un enlace de recuperación anteriormente. Por favor, revisa tu correo.");
                    return View(model);
                }

                if (user != null)
                {
                    try
                    {
                        // Generar una contraseña temporal
                        string tempPassword = GenerateTemporaryPassword(user.Nombre, user.Apellido, user.Cedula);
                        
                        // Usamos el método correcto de Identity para cambiar la contraseña
                        var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                        var result = await UserManager.ResetPasswordAsync(user.Id, token, tempPassword);

                        if (result.Succeeded)
                        {
                            try
                            {
                                // Enviar correo electrónico con la contraseña temporal
                                await SendPasswordResetEmail(user.Email, user.Nombre, tempPassword);

                                var recuperacion = new Recuperacion_Contra
                                {
                                    Id = user.Id
                                };

                                _context.Recuperacion_Contra.Add(recuperacion);

                                // Guardamos los cambios en la base de datos
                                await _context.SaveChangesAsync();

                                // Redirigir a la vista de confirmación sin mostrar la contraseña
                                return RedirectToAction("ForgotPasswordConfirmation", new { email = user.Email });
                            }
                            catch (Exception ex)
                            {
                                // Si falla el envío del correo, revertimos el cambio de contraseña
                                var resetResult = await UserManager.ResetPasswordAsync(user.Id, token, user.PasswordHash);
                                ModelState.AddModelError("", "Error al enviar el correo electrónico. Por favor, intente nuevamente más tarde.");
                                System.Diagnostics.Debug.WriteLine("Error al enviar correo: " + ex.Message);
                                System.Diagnostics.Debug.WriteLine("StackTrace: " + ex.StackTrace);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "No se pudo actualizar la contraseña. Por favor, intente nuevamente.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error al actualizar la contraseña: " + ex.Message);
                        System.Diagnostics.Debug.WriteLine("Error en ForgotPassword: " + ex.Message);
                        System.Diagnostics.Debug.WriteLine("StackTrace: " + ex.StackTrace);
                    }
                }
                else
                {
                    // No revelamos si el correo existe o no por seguridad
                    return RedirectToAction("ForgotPasswordConfirmation", new { email = model.Email });
                }
            }
           
            return View(model);
        }

        // Método para enviar el correo electrónico con la contraseña temporal
        private async Task SendPasswordResetEmail(string email, string nombre, string tempPassword)
        {
            try
            {
                // Obtener configuración SMTP del web.config
                string smtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
                int smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]);
                string smtpUser = System.Configuration.ConfigurationManager.AppSettings["SmtpUser"];
                string smtpPassword = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];

                // Verificar que la configuración sea válida
                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
                {
                    throw new Exception("Configuración SMTP incompleta en web.config");
                }

                System.Diagnostics.Debug.WriteLine($"Configuración SMTP: Servidor={smtpServer}, Puerto={smtpPort}, Usuario={smtpUser}");

                // Construir el contenido del correo con HTML
                var body = new StringBuilder();
                body.Append("<html><body style='font-family: Arial, sans-serif;'>");
                body.Append("<img src='https://i.imgur.com/QZbCmhl.png'>");
                body.Append("<h2>CentroIntegralSD</h2>");
                body.Append("<h2 style='color:#4CAF50;'>Recuperación de Contraseña</h2>");
                body.Append($"<p>Hola <strong>{nombre}</strong>,</p>");
                body.Append("<p>Hemos recibido una solicitud para restablecer tu contraseña en el sistema del Centro Integral Santo Domingo.</p>");
                body.Append("<p>Tu contraseña temporal es:</p>");
                body.Append($"<div style='background-color: #f5f5f5; padding: 10px; border-radius: 5px; margin: 10px 0; font-size: 18px; text-align: center;'>");
                body.Append($"<strong>{tempPassword}</strong>");
                body.Append("</div>");
                body.Append("<p>Por favor, inicia sesión con esta contraseña y cámbiala inmediatamente por una nueva.</p>");
                body.Append("<p>Si no solicitaste este cambio, por favor ignora este correo o contacta con el administrador del sistema.</p>");
                body.Append("<hr style='border: 1px solid #eee; margin: 20px 0;'>");
                body.Append("<p style='font-size: 12px; color: #777;'>Este es un correo automático, por favor no responda a este mensaje.</p>");
                body.Append("</body></html>");

                // Crear el mensaje
                using (var mensaje = new System.Net.Mail.MailMessage())
                {
                    mensaje.From = new System.Net.Mail.MailAddress(smtpUser, "Centro Integral Santo Domingo");
                    mensaje.To.Add(new System.Net.Mail.MailAddress(email));
                    mensaje.Subject = "Recuperación de Contraseña - Centro Integral Santo Domingo";
                    mensaje.Body = body.ToString();
                    mensaje.IsBodyHtml = true;

                    System.Diagnostics.Debug.WriteLine($"Enviando correo a: {email}");

                    // Configurar y enviar con SmtpClient
                    using (var smtp = new System.Net.Mail.SmtpClient())
                    {
                        smtp.Host = smtpServer;
                        smtp.Port = smtpPort;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
                        
                        System.Diagnostics.Debug.WriteLine("Iniciando envío de correo...");
                        await smtp.SendMailAsync(mensaje);
                        System.Diagnostics.Debug.WriteLine("Correo enviado exitosamente");
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar el error para depuración
                System.Diagnostics.Debug.WriteLine("Error al enviar correo: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("StackTrace: " + ex.StackTrace);
                
                // Lanzar una excepción más descriptiva
                throw new Exception("Error al enviar el correo de recuperación de contraseña. Por favor, contacte al administrador del sistema.", ex);
            }
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        // Método para generar una contraseña temporal aleatoria
        public static string GenerateTemporaryPassword(string nombre, string apellido, string cedula)
        {
            // Validar longitud mínima para evitar errores
            string nombreParte = nombre.Length >= 3 ? nombre.Substring(0, 3) : nombre;
            string apellidoParte = apellido.Length >= 2 ? apellido.Substring(0, 2) : apellido;
            string cedulaParte = cedula.Length >= 4 ? cedula.Substring(0, 4) : cedula;

            // Generar parte aleatoria
            string randomPart = GenerateRandomString(4);

            // Combinamos la base con la parte aleatoria y un símbolo al final
            return $"{nombreParte}{apellidoParte}{cedulaParte}{randomPart}!";
        }

        // Método para generar una cadena aleatoria con letras y números
        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[_random.Next(chars.Length)])
                .ToArray());
        }


        // Hashea la contraseña
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //------------------------------------------------------------------------------------------------------

        [AllowAnonymous]
        public ActionResult ResetPassword(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("", "Todos los campos son obligatorios.");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden.");
                ViewBag.Email = email;
                return View();
            }

            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = await UserManager.ResetPasswordAsync(user.Id, token, password);

                if (result.Succeeded)
                {
                    var recu = _context.Recuperacion_Contra.FirstOrDefault(r => r.Id == user.Id);

                    var historial = new Recuperacion_Historial
                    {
                        Recuperacion = "Contraseña Cambiada",
                        Id = user.Id
                    };

                    _context.Recuperacion_Historial.Add(historial);
                    _context.Recuperacion_Contra.Remove(recu);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }

                ViewBag.Email = email;
                return View();
            }

            ModelState.AddModelError("", "No se encontró un usuario con ese correo.");
            return View();
        }


        [AllowAnonymous]
        public async Task<ActionResult> ResetPasswordConfirmation()
        {
            return View();
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(Microsoft.AspNet.Identity.IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion



        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                // Obtener el ID del usuario actual
                string userId = User.Identity.GetUserId();

                // Buscar asignaciones temporales activas del usuario
                var asignacionesActivas = _context.AsignacionRolesTemporales
                    .Where(a => a.Id == userId)
                    .FirstOrDefault();


                if (asignacionesActivas != null) 
                {
                    // Obtener solo la fecha (sin la hora) para comparar
                    var fechaInicio = asignacionesActivas.Fecha_Inicio.Date;
                    var fechaFin = asignacionesActivas.Fecha_Fin.Date;
                    var fechaActual = DateTime.Now.Date;

                    // Si la fecha actual está entre la fecha de inicio y fin
                    if (fechaActual >= fechaInicio && fechaActual <= fechaFin)
                    {
                        asignacionesActivas.Estado = "Activo";
                    }
                    else
                    {
                        asignacionesActivas.Estado = "Inactivo";
                    }

                    // Guardar los cambios en la base de datos
                    _context.SaveChanges();
                }

                // Cerrar sesión
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log del error
                System.Diagnostics.Debug.WriteLine("Error en LogOff: " + ex.Message);
                
                // Cerrar sesión de todos modos
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Index", "Home");
            }
        }


    }
}
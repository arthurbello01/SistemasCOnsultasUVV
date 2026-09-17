using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaConsultasUVV.Data;
using SistemaConsultasUVV.Models;
using SistemaConsultasUVV.ViewModels;

namespace SistemaConsultasUVV.Controllers
{
    public class ContaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public ContaController(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // ---------------- CADASTRO ----------------

        [HttpGet]
        public IActionResult Registro() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
            if (emailExiste)
            {
                ModelState.AddModelError(nameof(model.Email), "Este e-mail já está cadastrado.");
                return View(model);
            }

            var usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                DataCadastro = DateTime.Now
            };

            // Hash da senha - nunca gravamos a senha em texto puro
            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, model.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Cadastro realizado com sucesso! Faça login para continuar.";
            return RedirectToAction(nameof(Login));
        }

        // ---------------- LOGIN ----------------

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(model);
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha);
            if (resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.Nome),
                new(ClaimTypes.Email, usuario.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Consultas");
        }

        // ---------------- LOGOUT ----------------

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AcessoNegado() => View();
    }
}

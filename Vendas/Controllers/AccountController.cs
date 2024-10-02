using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vendas.ViewModels.Account;

namespace Vendas.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var resultado = await _userManager.CreateAsync(usuario, model.Senha);

                if (resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Index", "home");
                }

                foreach (var erro in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro.Description);
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, model.Lembrarme, false);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Produtos");
                }

                ModelState.AddModelError(string.Empty, "Login Inválido");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");


        }

        public IActionResult Sair()
        {
            // Remover cookies de autenticação e sessão
            Response.Cookies.Delete("YourAuthenticationCookieName");
            Response.Cookies.Delete(".AspNetCore.Identity.Application");

            // Remover todos os cookies (se necessário)
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // Redirecionar para a página inicial ou de login
            return RedirectToAction("Index", "Home");
        }




    }
}

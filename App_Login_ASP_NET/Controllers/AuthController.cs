using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;

using App_Login_ASP_NET.Models;

using System.ComponentModel.DataAnnotations;

namespace App_Login_ASP_NET.Controllers
{

    // Apesar desta controller se chamar "Auth" as rotas utilizarão o nome "Account".

    [Route("Account")]
    public class AuthController : Controller
    {

        private readonly SignInManager<AppUser> _login_manager;

        private readonly UserManager<AppUser> _app_users_manager;

        public AuthController(SignInManager<AppUser> login_manager, UserManager<AppUser> app_users_manager)
        {

            this._login_manager = login_manager;

            this._app_users_manager = app_users_manager;

        }

        [HttpGet("Login")] // Nome da rota.
        public IActionResult Login()
        {

            return View();

        }

        [HttpPost("Login")] // Nome da rota.
        public async Task<IActionResult> Login([Required][EmailAddress] string Email, [Required] string Password, int Keep_Connected)
        {

            if (ModelState.IsValid)
            {

                AppUser? app_user = await this._app_users_manager.FindByEmailAsync(Email);

                if (app_user != null)
                {

                    Microsoft.AspNetCore.Identity.SignInResult login_result = await this._login_manager.PasswordSignInAsync(app_user, Password, Convert.ToBoolean(Keep_Connected), false);

                    if (login_result.Succeeded)
                    {

                        return RedirectToAction("Index", "Home");

                    }

                    else
                    {

                        ModelState.AddModelError(nameof(Email), "Login failed.");

                    }

                }

            }

            return View();

        }

        [HttpGet("Logout")] // Nome da rota.
        public async Task<IActionResult> Logout()
        {

            await this._login_manager.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }

        [HttpGet("AccessDenied")] // Nome da rota.
        public IActionResult AccessDenied(string ReturnUrl)
        {

            ModelState.AddModelError("", $"The route \"{ReturnUrl}\" is inaccessible.");

            return View();

        }

    }

}
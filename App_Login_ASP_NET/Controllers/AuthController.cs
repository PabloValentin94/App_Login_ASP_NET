using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace App_Login_ASP_NET.Controllers
{

    public class AuthController : Controller
    {

        public IActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public IActionResult Login([Required][EmailAddress] string Email, [Required] string Password)
        {

            if (ModelState.IsValid)
            {



            }

            return View();

        }

    }

}
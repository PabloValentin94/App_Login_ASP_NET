using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using App_Login_ASP_NET.Models;

namespace App_Login_ASP_NET.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {

        _logger = logger;

    }

    public IActionResult Index()
    {

        return View();

    }

    public IActionResult Privacy()
    {

        return View();

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    
    }

}
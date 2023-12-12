using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FieldRent.Models;
using FieldRent.Data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace FieldRent.Controllers;

public class HomeController : Controller
{



    private IMapRepository _mapRepository;
    private IUserRepository _userRepository;
    public HomeController(IUserRepository userRepository, IMapRepository mapRepository)
    {
        _userRepository = userRepository;
        _mapRepository = mapRepository;

    }

    public async Task<IActionResult> Index(int id)
    {

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

}

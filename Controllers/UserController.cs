using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FieldRent.Data.Abstract;
using FieldRent.Entity;
using FieldRent.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FieldRent.Controllers
{

    public class UserController : Controller
    {

        private IUserRepository _userRepository;
        private IMapRepository _mapRepository;
        public UserController(IUserRepository userRepository, IMapRepository mapRepository)
        {
            _userRepository = userRepository;
            _mapRepository = mapRepository;
        }





        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.Users.Include(x => x.Maps).ToListAsync();
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userRepository.Users.Include(x => x.Maps).FirstOrDefaultAsync(x => x.UserId == id);
            return View(user);
        }



        [HttpGet]
        public async Task<IActionResult> Connetion_maps(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userRepository.Users.Select(x => new MapEditViewModel
            {
                UserId = x.UserId,
                MapCheckIds = x.Maps,
            }).FirstOrDefaultAsync(p => p.UserId == id);
            ViewBag.xxx = await _mapRepository.Maps.Where(x => x.UserId == id).ToListAsync(); //.Where(x=>x.UserId!=id) tüm liste için sil

            return View(user);

        }


        [HttpPost]
        public IActionResult Connetion_maps(MapEditViewModel model, int[] MapIds)
        {


            var entity = _userRepository.Users.Include(i => i.Maps).FirstOrDefault(m => m.UserId == model.UserId);

            if (entity == null)
            {
                return NotFound();
            }



            var Enum_Maps = MapIds.Select(id => _mapRepository.Maps.Include(i => i.Requests).FirstOrDefault(i => i.MapId == id)).ToList();
            var ReqDeletePrice = 0;
            var deletePrice = 0;
            foreach (var item in Enum_Maps)
            {
                ReqDeletePrice = 0;
                var listReq = item.Requests;
                foreach (var itemreq in listReq)
                {
                    ReqDeletePrice += itemreq.RequestPrice;
                }
                deletePrice += item.MapPrice + ReqDeletePrice;

                _mapRepository.EditMap3reqdelete(

                    new Map
                    {
                        Requests = item.Requests,
                        MapId = item.MapId,
                        MapIsActive = true
                    }
                );





            }



            _userRepository.EditUser(
                new User
                {

                    UserId = model.UserId,
                    Maps = MapIds.Select(id => _mapRepository.Maps.FirstOrDefault(i => i.MapId == id)).ToList()

                }
            );


            _userRepository.EditUserPrice(
                new User
                {
                    UserPrice = -deletePrice,/////////////////////////////////////////////////////////////////////////////////////////////////////////
                    UserId = model.UserId,


                }
            );

            return RedirectToAction("Index");
        }









        [HttpGet]
        public async Task<IActionResult> FirstUserMap(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userRepository.Users.Select(x => new MapEditViewModel
            {
                UserId = x.UserId,
                MapCheckIds = x.Maps,
            }).FirstOrDefaultAsync(p => p.UserId == id);
            ViewBag.xxx = await _mapRepository.Maps.Where(x => x.UserId != id && x.MapIsActive == true).ToListAsync(); //.Where(x=>x.UserId!=id) tüm liste için sil

            return View(user);

        }





        [HttpPost]
        public IActionResult FirstUserMap(MapEditViewModel model, int[] MapIds)
        {


            var entity = _userRepository.Users.Include(i => i.Maps).FirstOrDefault(m => m.UserId == model.UserId);

            if (entity == null)
            {
                return NotFound();
            }

            var Enum_Maps = MapIds.Select(id => _mapRepository.Maps.FirstOrDefault(i => i.MapId == id)).ToList();

            var newPrice = 0;
            foreach (var item in Enum_Maps)
            {
                _mapRepository.EditMap2(

                    new Map
                    {
                        MapId = item.MapId,
                        MapIsActive = false
                    }
                );

                newPrice += item.MapPrice;
            }

            _userRepository.EditUser2(
                new User
                {
                    UserPrice = newPrice,
                    UserId = model.UserId,
                    Maps = MapIds.Select(id => _mapRepository.Maps.FirstOrDefault(i => i.MapId == id)).ToList()
                }
            );


            var EnumIds = new List<int>();
            if (Enum_Maps.Count > 1)
            {
                foreach (var item in Enum_Maps)
                {
                    EnumIds.Add(item.MapId);
                }

                return RedirectToAction("Multiple_Map_Requests", "Map", new { ids = EnumIds });
            }



            var mid = Enum_Maps[0].MapId;
            return RedirectToAction("Map_Requests", "Map", new { id = mid });
        }











        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)    //user girişi yoksa
            {
                return RedirectToAction("Index", "Map");
            }
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                if (isUser != null)
                {
                    var userClaims = new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));

                    if (isUser.Email == "info@sadikturan.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Map");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
                }
            }

            return View(model);
        }































    }
}
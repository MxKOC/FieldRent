using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FieldRent.Data.Abstract;
using FieldRent.Entity;
using FieldRent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FieldRent.Controllers
{

    public class AdminController : Controller
    {

        private IMapRepository _mapRepository;
        private IUserRepository _userRepository;
        private IFieldRepository _fieldRepository;
        public AdminController(IUserRepository userRepository, IMapRepository mapRepository, IFieldRepository fieldRepository)
        {
            _mapRepository = mapRepository;
            _fieldRepository = fieldRepository;
            _userRepository = userRepository;

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

        public IActionResult CreateField()
        {
            if (User.Identity!.IsAuthenticated)   ////////// Giriş ve admin kontrolü
            {
                return RedirectToAction("Login", "Login");

            }

            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }



        [HttpPost]
        public IActionResult CreateField(int number)
        {


            _fieldRepository.CreateField(
                                   new Field
                                   {
                                       FieldCoordinate = "x1",
                                       MapNumber = number,
                                   }

                            );



            Random rnd = new Random();
            var randomint = 0;

            for (int i = 0; i < number; i++)
            {
                _mapRepository.CreateMap(
                       new Map
                       {
                           MapCoordinate = "x1" + i,
                           MapPrice = randomint = rnd.Next(0, 500),
                           MapCondition = "Nadas",
                           MapIsActive = true,
                           FieldId = _fieldRepository.Fields.OrderBy(f => f.FieldId).LastOrDefault().FieldId
                       }

                );
            }


            return RedirectToAction("Index", "Map");
        }





        [HttpGet]
        public async Task<IActionResult> DeleteMap(int? id)
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
        public IActionResult DeleteMap(MapEditViewModel model, int[] MapIds)
        {


            var entity = _userRepository.Users.Include(i => i.Maps).FirstOrDefault(m => m.UserId == model.UserId);

            if (entity == null)
            {
                return NotFound();
            }



            var Enum_Maps = MapIds.Select(id => _mapRepository.Maps.Include(i => i.Requests).FirstOrDefault(i => i.MapId == id)).ToList();
            foreach (var item in Enum_Maps)
            {
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



            return RedirectToAction("Index");
        }





















        [HttpGet]
        public async Task<IActionResult> FirstUserMap(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var times = Enum.GetValues(typeof(Duration)).Cast<Duration>();
            ViewBag.TimeList = new SelectList(times);

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


            var time = model.Time;
            var newtime = DateTime.Now;


            if (time == Duration.Daily)
            {
                newtime = DateTime.Now.AddDays(1);
            }
            else if (time == Duration.Weekly)
            {
                newtime = DateTime.Now.AddDays(7);
            }
            else if (time == Duration.Mountly)
            {
                newtime = DateTime.Now.AddMonths(1);
            }
            else if (time == Duration.Yearly)
            {
                newtime = DateTime.Now.AddYears(1);
            }

            /*int option = time;

            DateTime newtime;

            switch (option)
            {
                case 1:
                    newtime = DateTime.Now.AddYears(1);
                    break;
                case 2:
                    newtime = DateTime.Now.AddMonths(1);
                    break;
                case 3:
                    newtime = DateTime.Now.AddDays(7);
                    break;
                case 4:
                    newtime = DateTime.Now.AddDays(1);
                    break;
                default:
                    newtime = DateTime.Now;
                    break;
            }



            var newtime = DateTime.Now.AddMonths(1);
            var newtime = DateTime.Now.AddDays(7);
            var newtime = DateTime.Now.AddDays(1);

            
*/



            var Enum_Maps = MapIds.Select(id => _mapRepository.Maps.FirstOrDefault(i => i.MapId == id)).ToList();
            foreach (var item in Enum_Maps)
            {


                _mapRepository.EditMap6Duration(

                    new Map
                    {
                        MapId = item.MapId,
                        Time = time,
                        MapStart = DateTime.Now,
                        MapStop = newtime
                    }
                );



                _mapRepository.EditMap2(

                    new Map
                    {
                        MapId = item.MapId,
                        MapIsActive = false
                    }
                );

            }

            _userRepository.EditUser2(
                new User
                {

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











    }
}
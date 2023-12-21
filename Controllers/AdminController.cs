using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FieldRent.Data.Abstract;
using FieldRent.Entity;
using FieldRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FieldRent.Controllers
{
    //[Authorize(Roles = "admin")]   ///////////////////////////Başka user giriş yapıp tıklayınca patlıyor
    public class AdminController : Controller
    {


        private IRequestRepository _requestRepository;
        private IMapRepository _mapRepository;
        private IUserRepository _userRepository;
        private IFieldRepository _fieldRepository;
        public AdminController(IUserRepository userRepository, IMapRepository mapRepository, IFieldRepository fieldRepository, IRequestRepository requestRepository)
        {
            _mapRepository = mapRepository;
            _fieldRepository = fieldRepository;
            _userRepository = userRepository;
            _requestRepository = requestRepository;
        }








        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.Users.Where(x=>x.IsAdmin==false).Include(x => x.Maps).ToListAsync();
            return View(users);
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userRepository.Users.Include(x => x.Maps).FirstOrDefaultAsync(x => x.UserId == id);
            return View(user);
        }

        public IActionResult CreateField()
        {






            //if (User.IsInRole("admin"))
            //{
                return View();
           // }

            //return RedirectToAction("Index", "Home");

        }



        [HttpPost]
        [Authorize(Roles = "admin")]
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


            return RedirectToAction("IndexMap", "Admin");
        }





        [HttpGet]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

                return RedirectToAction("Multiple_Map_Requests", "Admin", new { ids = EnumIds });
            }



            var mid = Enum_Maps[0].MapId;
            return RedirectToAction("Map_Requests", "Admin", new { id = mid });
        }













        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////










        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexMap()
        {
            var maps = await _mapRepository.Maps.Include(x => x.Requests).Include(x => x.User).ToListAsync();
            return View(maps);
        }



        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DetailsMap(int? id)
        {
            var map = await _mapRepository.Maps.Include(x => x.User).Include(x => x.Requests).FirstOrDefaultAsync(x => x.MapId == id);
            return View(map);
        }





        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Map_Requests(int? id)  // Sadece 1 map seçildiğinde onun requestlerinin seçilmesi
        {
            if (id == null)
            {
                return NotFound();
            }
            var map = await _mapRepository.Maps.Select(x => new MapRequestEditModel
            {
                MapId = x.MapId,
                MapReqCheckIds = x.Requests,
            }).FirstOrDefaultAsync(p => p.MapId == id);
            ViewBag.yyy = await _requestRepository.Requests.ToListAsync();

            return View(map);

        }


        [HttpPost]
        public IActionResult Map_Requests(MapRequestEditModel model, int[] ReqIds)
        {


            var entity = _mapRepository.Maps.Include(i => i.Requests).FirstOrDefault(m => m.MapId == model.MapId);

            if (entity == null)
            {
                return NotFound();
            }


            _mapRepository.EditMap( //Map e requestleri yolluyor
                new Map
                {

                    MapId = model.MapId,
                    Requests = ReqIds.Select(id => _requestRepository.Requests.FirstOrDefault(i => i.RequestId == id)).ToList()
                }
            );





            return RedirectToAction("IndexMap");
        }















        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Multiple_Map_Requests(List<int> ids)// 1den fazla map seçildiğinde onun requestlerinin seçilmesi
        {
            if (ids == null)
            {
                return NotFound();
            }

            var maps = await _mapRepository.Maps
                .Where(x => ids.Contains(x.MapId)).ToListAsync();


            ViewBag.yyy = await _requestRepository.Requests.ToListAsync();

            var MapMultiIds = new List<int>();

            foreach (var item in maps)
            {
                MapMultiIds.Add(item.MapId);
            }

            return View(MapMultiIds);

        }


        [HttpPost]
        public IActionResult Multiple_Map_Requests(List<int> ids, int[] ReqIds)
        {

            foreach (var itemid in ids)
            {
                _mapRepository.EditMap(

                    new Map
                    {
                        MapId = itemid,
                        Requests = ReqIds.Select(id => _requestRepository.Requests.FirstOrDefault(i => i.RequestId == id)).ToList()
                    }
                );




            }


            return RedirectToAction("IndexMap");
        }



        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Change_Map(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var map = await _mapRepository.Maps.FirstOrDefaultAsync(p => p.MapId == id);
            ViewBag.mapslist = await _mapRepository.Maps.Where(x => x.UserId != id && x.MapIsActive == true).ToListAsync(); //.Where(x=>x.UserId!=id) tüm liste için sil
            ViewBag.maplist2 = await _mapRepository.Maps.Where(x => x.MapIsActive == true).ToListAsync();
            return View();
        }






        [HttpPost]
        public IActionResult Change_Map(int selectId, int Id)
        {
            var newmap = _mapRepository.Maps.Include(x => x.User).Include(x => x.Requests).FirstOrDefault(p => p.MapId == selectId);
            var oldmap = _mapRepository.Maps.Include(x => x.User).Include(x => x.Requests).FirstOrDefault(p => p.MapId == Id);


            _mapRepository.EditMap5addrequser(

                new Map
                {
                    MapId = newmap.MapId,
                    UserId = oldmap.UserId,
                    Requests = oldmap.Requests,
                    MapIsActive = false,
                    MapStart = DateTime.Now,
                    MapStop = oldmap.MapStop,
                    Time = oldmap.Time

                }
            );

            _mapRepository.EditMap4delrequser(

                  new Map
                  {
                      MapId = oldmap.MapId,
                      MapIsActive = true,
                      MapStart = null,
                      MapStop = null,
                      Time = null
                  }
              );

            return RedirectToAction("IndexMap");
        }


    }
}
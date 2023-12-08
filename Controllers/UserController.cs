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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FieldRent.Controllers
{

    public class UserController : Controller
    {

        private IRequestRepository _requestRepository;
        private IMapRepository _mapRepository;
        private IUserRepository _userRepository;
        private IFieldRepository _fieldRepository;
        public UserController(IUserRepository userRepository, IMapRepository mapRepository, IFieldRepository fieldRepository, IRequestRepository requestRepository)
        {
            _mapRepository = mapRepository;
            _fieldRepository = fieldRepository;
            _userRepository = userRepository;
            _requestRepository = requestRepository;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var maps = _mapRepository.Maps;


            maps = maps.Where(i => i.UserId == userId).Include(x => x.Requests).Include(x => x.User);

            return View(await maps.ToListAsync());
        }




        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var map = await _mapRepository.Maps.Include(x => x.User).Include(x => x.Requests).FirstOrDefaultAsync(x => x.MapId == id);
            return View(map);
        }














        [HttpGet]
        public async Task<IActionResult> Rent_Map(int? id)
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
        public IActionResult Rent_Map(MapEditViewModel model, int[] MapIds)
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



                //(Herbiri)Map in time MapStart MapStop ekle
                //(Herbiri)MapIsActive = false





            }

            //Gelen mapi veya map leri user a ekle



            var EnumIds = new List<int>();
            if (Enum_Maps.Count > 1)
            {
                foreach (var item in Enum_Maps)
                {
                    EnumIds.Add(item.MapId);
                }

                return RedirectToAction("Multiple_Add_Request", "User", new { ids = EnumIds });
            }



            var mid = Enum_Maps[0].MapId;
            return RedirectToAction("Add_Request", "User", new { id = mid });
        }






        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////Map kısmı 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////Map kısmı 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////Map kısmı 








        [HttpGet]
        public async Task<IActionResult> Add_Request(int? id)  // Sadece 1 map seçildiğinde onun requestlerinin seçilmesi
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
        public IActionResult Add_Request(MapRequestEditModel model, int[] ReqIds)
        {


            var entity = _mapRepository.Maps.Include(i => i.Requests).FirstOrDefault(m => m.MapId == model.MapId);

            if (entity == null)
            {
                return NotFound();
            }

            //Map e requestleri yolluyor





            return RedirectToAction("Index");
        }















        [HttpGet]
        public async Task<IActionResult> Multiple_Add_Request(List<int> ids)// 1den fazla map seçildiğinde onun requestlerinin seçilmesi
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
        public IActionResult Multiple_Add_Request(List<int> ids, int[] ReqIds)
        {

            foreach (var itemid in ids)
            {


                //Map e requestleri yolluyor


            }


            return RedirectToAction("Index");
        }













        [HttpGet]
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





            //yeni map e UserId Requests MapIsActive = false MapStart = DateTime.Now, MapStop = oldmap.MapStop, ve time gidiyor



            // Eski mapde isactive true  start stop time null oluyor



            return RedirectToAction("Index");
        }

















    }
}
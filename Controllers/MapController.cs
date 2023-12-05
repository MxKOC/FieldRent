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

    public class MapController : Controller
    {


        private IMapRepository _mapRepository;
        private IRequestRepository _requestRepository;
        private IUserRepository _userRepository;
        public MapController(IMapRepository mapRepository, IRequestRepository requestRepository, IUserRepository userRepository)
        {
            _mapRepository = mapRepository;
            _requestRepository = requestRepository;
            _userRepository = userRepository;
        }





        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var maps = await _mapRepository.Maps.Include(x => x.Requests).Include(x => x.User).ToListAsync();
            return View(maps);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var map = await _mapRepository.Maps.Include(x => x.User).Include(x => x.Requests).FirstOrDefaultAsync(x => x.MapId == id);
            return View(map);
        }





    [HttpGet]
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


        var entity = _mapRepository.Maps.Include(i => i.Requests).FirstOrDefault(m => m.MapId == model.MapId);// Gereksiz kaldı

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





        return RedirectToAction("Index");
    }















    [HttpGet]
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

        return View(map);
    }






    [HttpPost]
    public IActionResult Change_Map(int MapId, string MapCoordinate)
    {
        var newmap = _mapRepository.Maps.Include(x => x.User).Include(x => x.Requests).FirstOrDefault(p => p.MapCoordinate == MapCoordinate);
        var oldmap = _mapRepository.Maps.Include(x => x.User).Include(x => x.Requests).FirstOrDefault(p => p.MapId == MapId);


        _mapRepository.EditMap5addrequser(

            new Map
            {
                MapId = newmap.MapId,
                UserId = oldmap.UserId,
                Requests = oldmap.Requests,
                MapIsActive = false
            }
        );

        _mapRepository.EditMap4delrequser(

              new Map
              {
                  MapId = oldmap.MapId,
                  MapIsActive = true
              }
          );

        return RedirectToAction("Index");
    }
}
}
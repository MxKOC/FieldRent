using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FieldRent.Data.Abstract;
using FieldRent.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FieldRent.Controllers
{

    public class AdminController : Controller
    {

        private IMapRepository _mapRepository;
        private IFieldRepository _fieldRepository;
        public AdminController(IMapRepository mapRepository, IFieldRepository fieldRepository)
        {
            _mapRepository = mapRepository;
            _fieldRepository = fieldRepository;

        }



        public IActionResult Index()
        {
            return View();
        }



        public IActionResult CreateField()
        {
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
                           MapPrice = randomint  = rnd.Next(0, 500),
                           MapCondition = "Nadas",
                           MapIsActive = true,
                           FieldId = _fieldRepository.Fields.OrderBy(f => f.FieldId).LastOrDefault().FieldId
                       }

                );
            }


            return RedirectToAction("Index", "Map");
        }
    }
}
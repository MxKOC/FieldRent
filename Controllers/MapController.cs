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




    }
}
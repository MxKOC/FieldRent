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

        private IUserRepository _userRepository;
        private IMapRepository _mapRepository;
        public UserController(IUserRepository userRepository, IMapRepository mapRepository)
        {
            _userRepository = userRepository;
            _mapRepository = mapRepository;
        }







    }
}
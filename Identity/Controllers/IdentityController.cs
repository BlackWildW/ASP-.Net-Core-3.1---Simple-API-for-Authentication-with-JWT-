using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Helpers;
using Identity.Models;
using Identity.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class IdentityController : Controller
    {
        public IUserService userservice;
        public IdentityContext db;
        public IdentityController(IUserService userService, IdentityContext db)
        {
            db = db;
            userservice = userService;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Authenticate([FromForm] Auth auth)
        {
            try
            {
                var LoginUser = userservice.Login(auth.Login, auth.Password);
                return Ok(LoginUser);
            }
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message});

            }
        }
        [HttpPost]
        public IActionResult Registration([FromForm] User users)
        {
            try
            {
                var user = userservice.Registration(users.Username, users.Login, users.Password);
                return Ok(user);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userservice.GetAll();
            return Ok(users);
        }
    }
}
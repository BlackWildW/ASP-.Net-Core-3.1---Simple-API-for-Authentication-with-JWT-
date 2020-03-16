using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Entities;
using Identity.Helpers;
using Identity.Models;
using Identity.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class IdentityController : Controller
    {
        public IUserService userservice;
        public IdentityContext db;
        public IdentityController(IUserService userService)
        {
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
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromForm] Auth auth)
        {
            try
            {
                var LoginUser = userservice.Login(auth.Email, auth.Password);
                return Ok(LoginUser);
            }
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message});

            }
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Registration([FromForm] User users)
        {
            try
            {
                var user = userservice.Registration(users.Name, users.Email, users.Password,users.ConfirmPassword);
                return Ok(user);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userservice.GetAll();
            return Ok(users);   
        }
    }
}
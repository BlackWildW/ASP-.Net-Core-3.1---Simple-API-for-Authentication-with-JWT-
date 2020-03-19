using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Entities;
using Identity.Helpers;
using Identity.Models;
using Identity.UserService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
        [Authorize(Role.Admin)]
        public IActionResult ForAdmin(User user)
        {
            return View(user);  
        }
        [Authorize(Role.Admin)]
        public IActionResult EditPage(User user)
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
                if(LoginUser != null)
                {
                    var tokenStr = userservice.GenerateJSONWebToken(LoginUser);
                    HttpContext.Session.SetString("JWT", tokenStr);
                }
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
        [Authorize(Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userservice.GetAll();
            return View("~/Views/Home/ForAdmin.cshtml",users);   
        }
        [Authorize(Role.Admin)]
        public IActionResult Edit(User user)
        {
            var editUser = userservice.GetById(user.Id);
             //editUser = userservice.Edit(user);
            return View("~/Views/Home/EditPage.cshtml", editUser);
        }
        [Authorize(Role.Admin)]
        public IActionResult Update(User user)
        {
            var updateUser = userservice.Update(user);
            return Ok(updateUser);
        }
        [Authorize(Role.Admin)]
        public IActionResult Delete(User user)
        {
            userservice.Delete(user);
            return View("~/Views/Home/ForAdmin.cshtml");
        }
    }
}
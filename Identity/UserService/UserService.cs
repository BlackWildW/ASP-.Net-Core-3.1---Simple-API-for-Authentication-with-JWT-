﻿using Identity.Helpers;
using Identity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.UserService
{
    
    public class UserService : IUserService
    {
        private AppSetting setting;
        private IdentityContext db;
        //private List<Auth> _users = new List<Auth>();

        public User Registration(string username, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                 throw new AppException("Username or login or password is require");
            if (db.Users.Any(x => x.Name == username && x.Email == email && x.Password == password))
                throw new AppException("Username or login or password is already taken");

            db.Users.Add(new User() { Name = username, Email = email, Password = password , ConfirmPassword = confirmPassword});
            db.SaveChanges();
            var user = db.Users.SingleOrDefault(x => x.Name == username && x.Email == email && x.Password == password);
            if (user == null)
                return null;

           // _users.Add(new User() { Username = username,Login = login, Password = password} );
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(setting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);
            db.SaveChanges();
            return user.WithoutPassword();
        }
        public User Login(string email,string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = db.Users.SingleOrDefault(x => x.Email == email && x.Password == password);
            if (user == null)
                throw new AppException("Incorrect login or password");
            return user.WithoutPassword();
        }


        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }
        public User GetById(int id)
        {
            var user = db.Users.SingleOrDefault(x => x.Id == id);
            return user.WithoutPassword();
        }
        public UserService(IOptions<AppSetting> options, IdentityContext db)
        {
            this.db = db;
            setting = options.Value;
        }
    }

    public interface IUserService
    {
        User Registration(string username, string login, string password, string confirmPassword);
        User Login(string login, string password);
        IEnumerable<User> GetAll();
        User GetById(int Id);
    }

}

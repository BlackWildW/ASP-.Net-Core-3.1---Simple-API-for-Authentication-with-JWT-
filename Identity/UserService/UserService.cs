using Identity.Entities;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public User Registration(string username, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new AppException("Username or login or password is require");
            if (db.Users.Any(x => x.Name == username && x.Email == email && x.Password == password))
                throw new AppException("Username or login or password is already taken");

            db.Users.Add(new User() { Name = username, Email = email, Password = password, ConfirmPassword = confirmPassword, Role = Role.User });
            db.SaveChanges();
            var user = db.Users.SingleOrDefault(x => x.Name == username && x.Email == email && x.Password == password);
            if (user == null)
                return null;
            return user;
        }
        public User Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = db.Users.SingleOrDefault(x => x.Email == email && x.Password == password);
            if (user == null)
                throw new AppException("Incorrect login or password");
            return user;
        }


        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }
        public User GetById(int id)
        {
            var user = db.Users.SingleOrDefault(x => x.Id == id);
            return user;
        }
        public string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(setting.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: setting.Issuer,
                audience: setting.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;


            //    //var tokenHandler = new JwtSecurityTokenHandler();
            //    //var key = Encoding.ASCII.GetBytes(setting.Secret);
            //    //var tokenDescriptor = new SecurityTokenDescriptor
            //    //{
            //    //    Subject = new ClaimsIdentity(new Claim[]
            //    //    {
            //    //        new Claim(ClaimTypes.Name, user.Id.ToString())
            //    //    }),

            //    //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //    //};
            //    ////var token = tokenHandler.CreateToken(tokenDescriptor);
            //    ////user = tokenHandler.WriteToken(token);
            //    //var encodetoken = tokenHandler.WriteToken();
            //    //return user;
            //}
        }

        public User Update(User user)
        {
            db.Users.Update(user);
            db.SaveChanges();
            return user;
        }
        public IEnumerable<User> Delete(User user)
        {
            db.Users.Remove(db.Users.FirstOrDefault(x => x.Id == user.Id));
            db.SaveChanges();
            return db.Users;
        }


        public UserService(IOptions<AppSetting> options, IdentityContext db,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            this.db = db;
            setting = options.Value;
        }
    }

    public interface IUserService
    {
        User Registration(string username, string login, string password, string confirmPassword);
        User Login(string login, string password);
        string GenerateJSONWebToken(User user);
        IEnumerable<User> GetAll();
        User Update(User user);
        IEnumerable<User> Delete(User user);
        User GetById(int Id);
    }
}

    

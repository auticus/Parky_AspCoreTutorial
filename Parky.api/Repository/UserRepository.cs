using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Parky.api.Data;
using Parky.api.Models;
using Parky.api.Repository.Interfaces;

namespace Parky.api.Repository
{
    public class UserRepository :IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }


        public bool IsUniqueUser(string userName)
        {
            var user = _db.Users.SingleOrDefault(p => p.UserName == userName);
            return user == null;
        }

        public User Authenticate(string userName, string password)
        {
            //if user is authenticated we want to create a token and pass it back to the API
            var user = _db.Users.SingleOrDefault(x => x.UserName == userName && x.Password == password);
            if (user == null) return null;

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role), 
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature )
            };

            var token = handler.CreateToken(tokenDescriptor);
            user.Token = handler.WriteToken(token);

            user.Password = string.Empty; //do not pass back the password
            return user;
        }

        public User Register(string userName, string password)
        {
            var user = new User()
            {
                UserName = userName,
                Password = password
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            user.Password = string.Empty; //clear the password to not pass back
            return user;
        }
    }
}

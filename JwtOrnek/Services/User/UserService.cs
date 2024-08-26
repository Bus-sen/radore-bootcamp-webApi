using JwtOrnek.Entities;
using JwtOrnek.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtOrnek.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppSettings appSettings;
        private readonly ApplicationDbContext _dbContext;

        //constructor injection
        public UserService(AppSettings appSettings, ApplicationDbContext dbContext)
        {
            this.appSettings = appSettings;
            this._dbContext = dbContext;
        }
        public (string username, string token)? Authenticate(string username, string password)
        {
            var user = _dbContext.ApplicationUsers.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (user == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey); //tokenı imzalayacak kişi olarak key hazırladık

            //tokenı var olan kullanıcı için burası çalışacak:
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                //Özel olarak şu claimler olsun, token içindeki bilgilerden
                //kişiye daha önce tokenı vermiş olduğumuz kişi olup olmadığını
                //anlamamız için gerekli olan değişkenler
                Subject = new ClaimsIdentity(new[]
                    {
                    new Claim("userId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),

                //token hangi tarihe kadar geçerli olacak:
                Expires= DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            //token ilk kez oluşturuluyorsa:
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string generatedToken = tokenHandler.WriteToken(token);

            //tuple metot geriye 2 tane sonuç döndük:
            return (user.UserName, generatedToken);
        }
    }
}

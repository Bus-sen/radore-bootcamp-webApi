using JwtOrnek.Models;
using JwtOrnek.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtOrnek.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class SamplesController : Controller
    {
        private static readonly string[] sehirler = { "İstanbul", "Ankara", "İzmir", "Bursa", "Adana", "Eskişehir" };
        private readonly IUserService _userService;
        public SamplesController (IUserService userService)
        {
            _userService = userService;
        }
        //aynı ifade farklı yazış:
        /*public SamplesController(IUserService userService) => _userService = userService;*/


        [HttpGet]
        public IActionResult GetSehirler() => Ok(sehirler);

        /*
        [HttpGet]
        public IActionResult GetSehirler()
        {
            return Ok(sehirler);
        }
        */

        //Burada da AllowAnonymous attribute nü kullanarak bu seferde bu metoda herkesin erişebileceğini söylüyoruz:
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticationModel authenticateModel)
        {
            var user = _userService.Authenticate(authenticateModel.Username, authenticateModel.Password);

            if (user == null)
                return BadRequest("Username ve şifre hatalı!");

            return Ok(new { Username = user.Value.username, Token = user.Value.token });
        }
    }
}

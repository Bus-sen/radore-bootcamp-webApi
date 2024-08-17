using CodeFirstOrnek.Data;
using CodeFirstOrnek.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstOrnek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitapController : ControllerBase
    {
        ApplicationDbContext _context;

        //constructor injection
        public KitapController(ApplicationDbContext context)
        {
            _context = context;
        }

        //kitap controllerı postman ya da tarayıcıdan çağırdığımız zaman ilk olarak bu metot tetiklenir:
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kitap>>> kitaplariGetir()
        {
            List<Kitap> kitapListesi;
            kitapListesi = await _context.Kitap.ToListAsync();  //Select * from Kitap

            return kitapListesi;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Kitap>>> kitapEkle(Kitap kitap)
        {
            try
            {
                _context.Kitap.Add(kitap);  //insert into Kitap (kitapAdi,fiyat,sayfaSayisi) values(Algoritmalar,400,480)
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

    }
}

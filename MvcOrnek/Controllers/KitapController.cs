using Microsoft.AspNetCore.Mvc;
using MvcOrnek.Models;
using Newtonsoft.Json;
using System.Drawing;
using System.Net;
using System.Text;

namespace MvcOrnek.Controllers
{
    public class KitapController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Kitap> kitapList = new List<Kitap>();
            using (var httpClient = new HttpClient())   //chromedaki gibi sekme açıyorum ve oraya gitmek istediğim sayfayı yazıyorum
            {
                using (var gelenYanit = await httpClient.GetAsync("https://localhost:7163/api/Kitap"))
                {
                    string gelenString = await gelenYanit.Content.ReadAsStringAsync();
                    kitapList = JsonConvert.DeserializeObject<List<Kitap>>(gelenString);
                }
            }
            return View(kitapList);
        }

        public ViewResult KitapOlustur() => View();
        /*public ViewResult KitapOlustur()
        {
            return View();
        }*/

        [HttpPost]
        public async Task<IActionResult> KitapOlustur(Kitap kitap)
        {
            Kitap eklenecekKitap = new Kitap();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent serializeEdilecekFilm = new StringContent(JsonConvert.SerializeObject(kitap),
                        Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:7163/api/Kitap", serializeEdilecekFilm))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            //işlem başarılı mesajını göster
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Index");   //işlem başarılıysa buraya git
        }
        
        [HttpGet]
        public async Task<IActionResult> GetKitap(int id)
        {
            Kitap gelenKitapDetay = new Kitap();
            using (var httpClient = new HttpClient())
            {
                using (var gelenYanit = await httpClient.GetAsync("https://localhost:7163/api/Kitap/" +id))
                {
                    string gelenKitapDetayString = await gelenYanit.Content.ReadAsStringAsync();
                    gelenKitapDetay = JsonConvert.DeserializeObject<Kitap>(gelenKitapDetayString);
                }
            }
            return View(gelenKitapDetay);
        }
        
        // ----------- DELETE ------------
        public async Task<IActionResult> KitapSil (int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7163/api/");
                var silinecekKitap = httpClient.DeleteAsync("Kitap/" + id);
                silinecekKitap.Wait();
                using (var response = silinecekKitap.Result)
                {
                    if (response.IsSuccessStatusCode) { }
                        //return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

        }

        // ----------- UPDATE -------------
        public async Task<ViewResult> KitapGuncelle (int id)
        {
            Kitap gelenKitapDetay = new Kitap();
            using (var httpClient = new HttpClient())
            {
                using (var gelenYanit = await httpClient.GetAsync("https://localhost:7163/api/Kitap/" + id))
                {
                    string gelenKitapDetayString = await gelenYanit.Content.ReadAsStringAsync();
                    gelenKitapDetay = JsonConvert.DeserializeObject<Kitap>(gelenKitapDetayString);
                }
            }
            return View(gelenKitapDetay);
        }

        [HttpPost]
        public async Task<IActionResult> KitapGuncelle(Kitap kitap)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent guncellenecekKitap = new StringContent(JsonConvert.SerializeObject(kitap),
                        Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:7163/api/Kitap", guncellenecekKitap))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //işlem başarılı mesajı
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}

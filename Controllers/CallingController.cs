using DrugMMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DrugMMvc.Controllers
{
    public class CallingController : Controller
    {

        string baseURL = "https://localhost:7289/";
        public async Task<IActionResult> Index()
        {
            List<Product> p = new List<Product>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/Products");
                if (res.IsSuccessStatusCode)
                {
                    var Prodres = res.Content.ReadAsStringAsync().Result;
                    p = JsonConvert.DeserializeObject<List<Product>>(Prodres);
                }
                return View(p);
            }
        }
        public IActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product pizza)
        {
           Product pizzaobj = new Product();
            using (var httpClient = new HttpClient())
            {
        StringContent content = new StringContent(JsonConvert.SerializeObject(pizza), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7289/api/Products", content);
                //{
                //    string apiResponse = await response.Content.ReadAsStringAsync();
                //    pizzaobj = JsonConvert.DeserializeObject<Product>(apiResponse);
                //}
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Product obj = new Product();
            using (var client = new HttpClient())
            {
                using (var ares = await client.GetAsync("https://localhost:7289/api/Products/" + id))
                {
                    string apires = await ares.Content.ReadAsStringAsync();
                    obj = JsonConvert.DeserializeObject<Product>(apires);
                }
            }
            return View(obj);
        }
        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            Product p1 = new Product();
            TempData["id"]=id;
            using (var client = new HttpClient())
            {
                using (var res = await client.GetAsync("https://localhost:7289/api/Products/" + id))
                {
                    string ar = await res.Content.ReadAsStringAsync();
                    p1 = JsonConvert.DeserializeObject<Product>(ar);
                }
            }
            return View(p1);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            Product p2 = new Product();
            int id = p.ProductId;
            using (var client = new HttpClient())
            {
               

                StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
                using (var res = await client.PutAsync("https://localhost:7289/api/Products/" + id, content))
                {
                    string apr = await res.Content.ReadAsStringAsync();
                    p2 = JsonConvert.DeserializeObject<Product>(apr);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product p3 = new Product();
            TempData["id"] = id;
            using (var client = new HttpClient())
            {
                using (var res = await client.GetAsync("https://localhost:7289/api/Products/" + id))
                {
                    string apir = await res.Content.ReadAsStringAsync();
                    p3 = JsonConvert.DeserializeObject<Product>(apir);
                }
            }
            return View(p3);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(Product products)
        {
            int id = (int)TempData["id"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                await client.DeleteAsync("https://localhost:7289/api/Products/" + id);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AdminIndex()
        {
            List<Product> p = new List<Product>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/Products");
                if (res.IsSuccessStatusCode)
                {
                    var Prodres = res.Content.ReadAsStringAsync().Result;
                    p = JsonConvert.DeserializeObject<List<Product>>(Prodres);
                }
                return View(p);
            }
        }

    }
}

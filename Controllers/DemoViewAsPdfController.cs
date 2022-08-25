using DrugMMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Rotativa.AspNetCore;
namespace DrugMMvc.Controllers
{
    public class DemoViewAsPdfController : Controller
    {
        public async Task<IActionResult> DemoViewAsPdf ()
        {
            var UserId = HttpContext.Session.GetInt32("id");
            List<OrderDetail> od = new List<OrderDetail>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/Methods/" + UserId);
                if (res.IsSuccessStatusCode)
                {
                    var Prodres = res.Content.ReadAsStringAsync().Result;
                    od = JsonConvert.DeserializeObject<List<OrderDetail>>(Prodres);
                }
            }
            List<Buyer> buyer = new List<Buyer>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/Transactions/" + UserId);
                if (res.IsSuccessStatusCode)
                {
                    var Prodres1 = res.Content.ReadAsStringAsync().Result;
                    buyer = JsonConvert.DeserializeObject<List<Buyer>>(Prodres1);
                }
            }
            var p2 = buyer.LastOrDefault();
            List<Product> products = new List<Product>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/Products");
                if (res.IsSuccessStatusCode)
                {
                    var Prodres = res.Content.ReadAsStringAsync().Result;
                    products = JsonConvert.DeserializeObject<List<Product>>(Prodres);
                }
            }
            List<OrderDetail> p3 = (from i in od where i.PurchaseId == p2.PurchaseId select i).ToList();
            int l = p3.Count();
            var a = new List<object>();
            foreach (var item in p3)
            {
                foreach (var item2 in products)
                {
                    if (item.ProductId == item2.ProductId)
                        a.Add(item2.ProductName);
                    // a.Add(item2.Price);

                }
            }
            var b = new List<object>();
            foreach (var item in p3)
            {
                foreach (var item2 in products)
                {
                    if (item.ProductId == item2.ProductId)
                        b.Add(item2.Price);
                    // a.Add(item2.Price);

                }
            }
            var c = new List<object>();
            foreach (var item in p3)
            {
                c.Add(item.Quantity);
            }


            ViewBag.Collection = a;
            ViewBag.Collection1 = b;
            ViewBag.Collection2 = c;
            // DateOnly date = DateOnly.FromDayNumber
            string Today = DateTime.Now.AddDays(10).ToString("dd-MM-yyyy");
            ViewBag.Date = Today;
            ViewBag.Total = HttpContext.Session.GetInt32("t");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Location = HttpContext.Session.GetString("Location");
            ViewBag.PhNum = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.EmailId = HttpContext.Session.GetString("EmailId");
            return new ViewAsPdf();
           
        }
    }
}

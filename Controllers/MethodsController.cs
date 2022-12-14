using DrugMMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace DrugMMvc.Controllers
{
    public class MethodsController : Controller
    {
        string baseURL = "https://localhost:7289/";

        [NoDirectAccess]

        public async Task<IActionResult> AddCart(int id)
        {
            TempData["id"] = id;
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

                foreach (var a in p)
                {
                    if (a.ProductId == id)
                    {
                        HttpContext.Session.SetString("Name", a.ProductName);

                        ViewBag.ProductName = a.ProductName;
                        ViewBag.Price = a.Price;
                        ViewBag.Stock = a.Stock;
                        ViewBag.ProductId = a.ProductId;

                    }
                }

                Product p1 = new Product();
                //HttpContext.Session.SetInt32("stock", (int)p1.Stock);
                p1.ProductId = id;
                return View(p1);
            }
        }

        [NoDirectAccess]

        [HttpPost]


        public async Task<IActionResult> AddCart(Product p)
        {

            OrderDetail? O = new OrderDetail();
            O.UserId = (int)HttpContext.Session.GetInt32("id");
            O.ProductId = p.ProductId;

            O.ProductId = (int)TempData["id"];
            //int id = (int)TempData["id"];
            //O.ProductId = id;
            O.Quantity = p.Stock;

            O.PurchaseId = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseURL);
                StringContent content = new StringContent(JsonConvert.SerializeObject(O), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7289/api/Methods", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    O = JsonConvert.DeserializeObject<OrderDetail>(apiResponse);
                }
            }
            return RedirectToAction("Cart");
        }
        [NoDirectAccess]

        public async Task<IActionResult> Cart()
        {
            int id = (int)HttpContext.Session.GetInt32("id");
            List<OrderDetail> p = new List<OrderDetail>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/Methods/" + id);
                if (res.IsSuccessStatusCode)
                {
                    var Prodres = res.Content.ReadAsStringAsync().Result;
                    p = JsonConvert.DeserializeObject<List<OrderDetail>>(Prodres);
                }
            }

            List<Product> p1 = new List<Product>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/Products");
                if (res.IsSuccessStatusCode)
                {
                    var Prodres = res.Content.ReadAsStringAsync().Result;
                    p1 = JsonConvert.DeserializeObject<List<Product>>(Prodres);
                }
            }

            // List<OrderDetail> p4 = p.ToList(i => i.PurchaseId == null);
            //List<OrderDetail> p4= new List<OrderDetail>();
            //foreach (var item in p)
            //{
            //    p4= p.ToList(i => i.PurchaseId==null);

            //}




            foreach (var n in p)
            {
                if (n.PurchaseId == null)
                {
                    var p2 = p1.FirstOrDefault(i => i.ProductId == n.ProductId);
                    n.ProductName = p2.ProductName;
                    n.UnitPrice = p2.Price;
                }
                else
                {
                    n.Quantity = null;
                }
            }
            Nullable<int> t = 0;
            foreach (var n1 in p)
            {
                if (n1.PurchaseId == null)
                {
                    t += (n1.UnitPrice * n1.Quantity);
                }
            }

            ViewBag.Total = t;
            HttpContext.Session.SetInt32("t", (int)t);
            return View(p);
        }


        //[HttpGet]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    OrderDetail o = new OrderDetail();
        //    TempData["id"] = id;
        //    using (var client = new HttpClient())
        //    {
        //        using (var res = await client.GetAsync("https://localhost:7289/api//" + id))
        //        {
        //            string apir = await res.Content.ReadAsStringAsync();
        //            o = JsonConvert.DeserializeObject<OrderDetail>(apir);
        //        }
        //    }
        //    return View(o);

        //}

        [NoDirectAccess]

        public async Task<IActionResult> Delete(int OrderId)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                await client.DeleteAsync("https://localhost:7289/api/Methods/" + OrderId);
            }
            return RedirectToAction("Index", "Calling");
        }

        [NoDirectAccess]

        public async Task<IActionResult> Buy()
        {
            ViewBag.Total = HttpContext.Session.GetInt32("t");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Location = HttpContext.Session.GetString("Location");
            ViewBag.PhNum = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.EmailId = HttpContext.Session.GetString("EmailId");
            return View();

        }

        [NoDirectAccess]

        [HttpPost]
        public async Task<IActionResult> Buy(Buyer buyer)
        {
            buyer.DateofPurchase = DateTime.Now;
            buyer.TotalAmount = HttpContext.Session.GetInt32("t");
            buyer.UserId = (int)HttpContext.Session.GetInt32("id");
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(buyer), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7289/api/Methods", content);
                //{
                //    string apiResponse = await response.Content.ReadAsStringAsync();
                //    pizzaobj = JsonConvert.DeserializeObject<Product>(apiResponse);
                //}
            }


            return RedirectToAction("Thanking");
        }
        [NoDirectAccess]

        public async Task<IActionResult> Transactions()
        {
            List<Buyer> p = new List<Buyer>();
            var UserId = HttpContext.Session.GetInt32("id");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/Transactions/" + UserId);
                if (res.IsSuccessStatusCode)
                {
                    var Prodres = res.Content.ReadAsStringAsync().Result;
                    p = JsonConvert.DeserializeObject<List<Buyer>>(Prodres);
                }
               
            }
            List<String> s=new List<String>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("https://localhost:7289/api/GetProductName/GetProductName/" + UserId);
                if (res.IsSuccessStatusCode)
                {
                    var Prodres = res.Content.ReadAsStringAsync().Result;
                    s= JsonConvert.DeserializeObject<List<string>>(Prodres);
                }

            }
           



            var mode = new List<string>();
            foreach(var m in p)
            {
                if(m.PaymentMode!=null)
                {
                    mode.Add(m.PaymentMode);
                }
            }
            var mode1 = new List<object>();
            foreach (var m in p)
            {
                if (m.DateofPurchase != null)
                {
                   
                    mode1.Add(m.DateofPurchase);
                }
            }
            var mode2 = new List<object>();
            foreach (var m in p)
            {
                if (m.TotalAmount!= null)
                {
                    mode2.Add(m.TotalAmount);
                }
            }



            ViewBag.PName = s;
            ViewBag.DOP = mode1;
            ViewBag.PM=mode;
            ViewBag.TM=mode2;
            return View();
        }
        [NoDirectAccess]

        public async Task<IActionResult> Thanking()
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
            return View();
        }
        //  https://localhost:7289/api/GetCart/143
        [NoDirectAccess]

        public async Task<IActionResult> EditCart(OrderDetail order)
        {
            OrderDetail od = new OrderDetail();
            //
            TempData["id"] = (int)order.OrderId;
                using (var client1 = new HttpClient())
                {
                    using (var res1 = await client1.GetAsync(" https://localhost:7289/api/GetCart/" + order.OrderId))
                    {
                        string ar = await res1.Content.ReadAsStringAsync();
                        od = JsonConvert.DeserializeObject<OrderDetail>(ar);
                    }
                }
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
                
                foreach (var a in p)
                {
                    if (a.ProductId == od.ProductId)
                    {
                        //HttpContext.Session.SetString("Name", a.ProductName);


                        ViewBag.stock = a.Stock;


                    }
                }
                return View(od);
            }

        }

        [HttpPost]
        [Route("OrderDetail")]
        public async Task<IActionResult> EditingCart(OrderDetail order)
        {
            order.OrderId = (int)TempData["id"];
            OrderDetail od1 = new OrderDetail();
            using (var client1 = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                using (var res1 = await client1.PutAsync("https://localhost:7289/api/GetCart/OrderDetail",content))
                {
                    string ar = await res1.Content.ReadAsStringAsync();
                    od1 = JsonConvert.DeserializeObject<OrderDetail>(ar);
                }
            }
            return RedirectToAction("Cart");
        }


        #region LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
        #endregion
    }

}

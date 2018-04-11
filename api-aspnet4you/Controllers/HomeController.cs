using api.aspnet4you.mvc5.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace api.aspnet4you.mvc5.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client;
        string url = ConfigurationManager.AppSettings.Get("apiBaseUrl");
        string img01 = ConfigurationManager.AppSettings.Get("img01");
        string img02 = ConfigurationManager.AppSettings.Get("img02");

        public HomeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> Index()
        {
            ViewData["Message"] = "Welcome to api.aspnet4you.com!";

            ImageModel imgModel = null;
             HttpResponseMessage responseMessage = await client.GetAsync($"{url}/GetImage/{img02}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                imgModel = JsonConvert.DeserializeObject<ImageModel>(responseData);
            }

            return View(imgModel);
        }

        public ActionResult About()
        {
            return View();
        }

        public async Task<ActionResult> ShowCSharpBanner()
        {
            ImageModel imgModel = null;
            HttpResponseMessage responseMessage = await client.GetAsync($"{url}/GetImage/{img01}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                imgModel = JsonConvert.DeserializeObject<ImageModel>(responseData);
            }

            return File(imgModel.Data, imgModel.ContentType);
        }

        public async Task<ActionResult> ShowAppArchitecture()
        {
            ImageModel imgModel = null;
            HttpResponseMessage responseMessage = await client.GetAsync($"{url}/GetImage/{img02}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                imgModel = JsonConvert.DeserializeObject<ImageModel>(responseData);
            }

            return File(imgModel.Data, imgModel.ContentType);
        }
    }
}
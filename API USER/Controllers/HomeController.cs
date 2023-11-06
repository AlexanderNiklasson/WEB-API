using API_USER.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API_USER.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var c = _httpClientFactory.CreateClient("api");

            string apiEndpoint = "authors/All";

            var response = await c.GetAsync(apiEndpoint);

            if(response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();

                List<Author> authors = JsonSerializer.Deserialize<List<Author>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive= true
                });

                return View(authors);
            }
            else
            {
                return View("Error");
            }

        }
        [HttpGet]
        public async Task <IActionResult> Posts()
        {
            var c = _httpClientFactory.CreateClient("api");

            string apiEndpoint = "posts";

            var response = await c.GetAsync(apiEndpoint);


            if(response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
           
                List<Post> posts = JsonSerializer.Deserialize<List<Post>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(posts);
            }
            else
            {
                return View("Error");
            }

            
        }
        [HttpGet]
        public async Task <IActionResult> User(int id)
        {
            Console.WriteLine("Id  " + id );
            var c = _httpClientFactory.CreateClient("api");
            string apiEndpoint = "authors/" + id.ToString();

            var response = await c.GetAsync(apiEndpoint);

            
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{id.ToString()} {content}");
                Author a = JsonSerializer.Deserialize<Author>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return View(a);
            }
            else
            {
                return View("Err");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
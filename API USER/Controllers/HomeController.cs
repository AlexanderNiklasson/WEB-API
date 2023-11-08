using API_USER.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Text;

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
            HttpContext.Session.Clear();
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
        [HttpGet("InitializeSession/{id}")]
        public IActionResult InitializeSession(int id)
        {
            HttpContext.Session.SetInt32("LoggedInAuthor", id);
            
            return RedirectToAction("Posts");
        }

        [HttpGet("Posts")]
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
            var c = _httpClientFactory.CreateClient("api");
            string apiEndpoint = "authors/" + id.ToString();
            
            var response = await c.GetAsync(apiEndpoint);

            
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
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
        [HttpGet("DeletePost/{id}")]
        public IActionResult DeletePost(int id)
        {
            var c = _httpClientFactory.CreateClient("api");
            string apiEndpoint = "posts/" + id.ToString();

            var response = c.DeleteAsync(apiEndpoint).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("User", new { id = HttpContext.Session.GetInt32("LoggedInAuthor") });
            }
            else
            {
                return View("Error");
            }

            
        }
        [HttpGet("/CreatePost")]
        public IActionResult CreatePost()
        {
            var post = new Post();

            return View(post);            
        }
        [HttpPost("/CreatePost")]
        public async Task<IActionResult> SendPost(Post post)
        {
            var c = _httpClientFactory.CreateClient("api");
            post.AuthorId = (int)HttpContext.Session.GetInt32("LoggedInAuthor");
            string apiEndpoint = "posts";

            var json = JsonSerializer.Serialize(post);
            var body = new StringContent(json, Encoding.UTF8, "application/json");
            

            var response = await c.PostAsync(apiEndpoint, body);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("User", new { id = HttpContext.Session.GetInt32("LoggedInAuthor") });
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet("/UpdatePost/{id}")]
        public async Task<IActionResult> EditPost(int id)
        {
            var c = _httpClientFactory.CreateClient("api");
            string apiEndpoint = "posts/" + id.ToString();

            var response = await c.GetAsync(apiEndpoint);


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Post p = JsonSerializer.Deserialize<Post>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(p);
            }
            else
            {
                return View("Err");
            }

        }

        [HttpPost("/UpdatePost/{id}")]
        public async Task<IActionResult> SendEditPost(Post post, int id)
        {

       
            var c = _httpClientFactory.CreateClient("api");
            string apiEndpoint = "posts/"  + id.ToString();

            var json = JsonSerializer.Serialize(post);
            var body = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await c.PutAsync(apiEndpoint, body);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("User", new {id = HttpContext.Session.GetInt32("LoggedInAuthor")} );
            }
            else
            {
                return View("Error");
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        //cache
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }

        //få upp alla filmer
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var url = "https://ghibliapi.herokuapp.com/films/58611129-2dbc-4a81-a72f-77ddfc1b1b49";

            string serializedResponse;
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    var responseCont = response.Content;
                    var stringResp = await responseCont.ReadAsStringAsync();
                    serializedResponse = JsonConvert.SerializeObject(stringResp);
                }
            }

            return Ok(serializedResponse);
        }
    }
}

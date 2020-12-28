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

        //få upp filmen "Howl's moving castle"
        //routing
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var url = "https://ghibliapi.herokuapp.com/films";

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

        //få Title för film
        [HttpGet("{id}/{title}")]
        public async Task<IActionResult> Get(string id, string title)
        {
            var urlTitle = $"https://ghibliapi.herokuapp.com/films/id={id}/title={title}";
            string responseOutput;
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(urlTitle);
                    responseOutput = await response.Content.ReadAsStringAsync();
                    var desResponse = JsonConvert.DeserializeObject<dynamic>(responseOutput);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(responseOutput);
        }
    }
}

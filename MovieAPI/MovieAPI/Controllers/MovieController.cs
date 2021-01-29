using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MovieAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        //cache
        //FIXA CACHE
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var url = "https://ghibliapi.herokuapp.com/films";

            var stringResp = string.Empty;

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    var responseCont = response.Content;
                    stringResp = await responseCont.ReadAsStringAsync();
                }
            }
            var deserializedResponse = JsonConvert.DeserializeObject<List<Response>>(stringResp,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return Ok(deserializedResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var urlId = $"https://ghibliapi.herokuapp.com/films/{id}";
            Response responseOutput;
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(urlId);
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    responseOutput = JsonConvert.DeserializeObject<Response>(stringResponse,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(new { Data = responseOutput, StatusCode = HttpStatusCode.OK });
        }
    }
}
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

        //return all films
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var url = "https://ghibliapi.herokuapp.com/films";

            //string serializedResponse;
            var stringResp = string.Empty;
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    var responseCont = response.Content;
                    stringResp = await responseCont.ReadAsStringAsync();
                    //serializedResponse = JsonConvert.SerializeObject(stringResp);
                }
            }
            var deserializedResponse = JsonConvert.DeserializeObject<List<Response>>(stringResp,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return Ok(deserializedResponse);
        }

        //Returns a film on id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var urlId = $"https://ghibliapi.herokuapp.com/films/{id}";
            string responseOutput;
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(urlId);
                    responseOutput = await response.Content.ReadAsStringAsync();
                    var desResponse = JsonConvert.DeserializeObject<dynamic>(responseOutput);
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

//Ta bort post och delete, används inte här, bara på Movie_Console
//Add movies

//[HttpPost]
//public IActionResult AddMovies([FromBody] MovieModel payload)
//{
//  var responseModel = new { Data = payload, StatusCode = Ht };
//return Ok(JsonConvert.SerializeObject(payload));
//}

//Remove Movies
//[HttpDelete]
//public IActionResult DeleteMovies([FromBody] MovieModel payload)
//{
//  bool success = DeleteMovies(payload);
//    //return Ok(JsonConvert.SerializeObject(payload));
//}

//public bool DeleteMovie(MovieModel model)
//{
//  if(MovieList.Any(model.Id))
//{
// MovieList.Remove(x => x.Id == model.Id);
//  return true;
//}
//return false;


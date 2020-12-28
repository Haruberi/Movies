using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var url = "https://ghibliapi.herokuapp.com";

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

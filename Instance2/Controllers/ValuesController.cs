using Instance2.Reciver;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Instance2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(IMemoryCache cache, IReciver reciver) : ControllerBase
    {

        [HttpPost("SendValues")]
        public IActionResult SendValues(string key, string value)
        {
            cache.Set(key, value);
            reciver.ReciveKeyValue(key, value);
            return Ok();

        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FetchDataController : ControllerBase
    {
        [HttpGet(Name = "GetData")]
        public IEnumerable<SeedData> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new SeedData
            {
                ID = index,
                DOB = DateTime.Now.AddDays(index),
                Name = $"Name{index}",
            })
            .ToArray();
        }
    }

    public class SeedData
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime DOB { get; set; }
    }
}

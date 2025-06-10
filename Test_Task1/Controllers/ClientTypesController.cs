using Microsoft.AspNetCore.Mvc;

namespace test_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientTypesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetClientTypes()
        {
            var types = new List<string> { "ИП", "ЮЛ" };
            return Ok(types);
        }
    }
}
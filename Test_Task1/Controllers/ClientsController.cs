using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_task.models;

namespace test_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/clients?founderId=1
        [HttpGet]
        public async Task<IActionResult> GetMyClients([FromQuery] int founderId)
        {
            var founder = await _context.Founders
                .Include(f => f.Clients)
                .ThenInclude(c => c.Founders)
                .FirstOrDefaultAsync(f => f.Id == founderId);

            if (founder == null)
                return NotFound("Учредитель не найден");

            return Ok(founder.Clients);
        }

        // POST: api/clients?founderId=1
        [HttpPost]
        public async Task<IActionResult> AddOrLinkClient([FromQuery] int founderId, [FromBody] Clients inputClient)
        {
            if (inputClient.Type != "ИП" && inputClient.Type != "ЮЛ")
                return BadRequest("Тип клиента должен быть 'ИП' или 'ЮЛ'");

            var founder = await _context.Founders
                .Include(f => f.Clients)
                .FirstOrDefaultAsync(f => f.Id == founderId);

            if (founder == null)
                return NotFound("Учредитель не найден");

            var existingClient = await _context.Clients
                .Include(c => c.Founders)
                .FirstOrDefaultAsync(c => c.INN == inputClient.INN);

            if (inputClient.Type == "ИП")
            {
                if (existingClient != null && existingClient.Founders.Any())
                    return BadRequest("Этот ИП уже связан с другим учредителем");

                if (existingClient == null)
                {
                    inputClient.CreatedAt = DateTime.UtcNow;
                    inputClient.UpdatedAt = DateTime.UtcNow;
                    inputClient.Founders = new List<Founders> { founder };
                    _context.Clients.Add(inputClient);
                }
                else
                {
                    existingClient.Founders.Add(founder);
                    existingClient.UpdatedAt = DateTime.UtcNow;
                }
            }
            else // ЮЛ
            {
                if (existingClient == null)
                {
                    inputClient.CreatedAt = DateTime.UtcNow;
                    inputClient.UpdatedAt = DateTime.UtcNow;
                    inputClient.Founders = new List<Founders> { founder };
                    _context.Clients.Add(inputClient);
                }
                else
                {
                    if (!existingClient.Founders.Contains(founder))
                    {
                        existingClient.Founders.Add(founder);
                        existingClient.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Клиент успешно добавлен или привязан");
        }

        // PUT: api/clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditClient(int id, [FromBody] Clients updated)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound("Клиент не найден");

            client.Name = updated.Name;
            client.Type = updated.Type;
            client.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/clients/5?founderId=1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id, [FromQuery] int founderId)
        {
            var founder = await _context.Founders
                .Include(f => f.Clients)
                .FirstOrDefaultAsync(f => f.Id == founderId);

            if (founder == null)
                return NotFound("Учредитель не найден");

            var client = await _context.Clients
                .Include(c => c.Founders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                return NotFound("Клиент не найден");

            if (!client.Founders.Contains(founder))
                return Forbid("Вы не связаны с этим клиентом");

            // удаляем связь
            client.Founders.Remove(founder);

            // если клиент больше ни с кем не связан — удалить из БД
            if (!client.Founders.Any())
                _context.Clients.Remove(client);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
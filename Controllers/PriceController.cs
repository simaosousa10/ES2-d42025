using ESIID42025.Models;
using ESIID42025.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESIID42025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly PriceService _priceService;

        public PriceController(PriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Price>>> GetPrices()
        {
            var prices = await _priceService.GetAllAsync();
            return Ok(prices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Price>> GetPrice(int id)
        {
            var price = await _priceService.GetByIdAsync(id);
            if (price == null)
                return NotFound();

            return Ok(price);
        }

        [HttpPost]
        public async Task<ActionResult<Price>> PostPrice(Price price)
        {
            var created = await _priceService.CreateAsync(price);
            return CreatedAtAction(nameof(GetPrice), new { id = created.ID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrice(int id, Price price)
        {
            var success = await _priceService.UpdateAsync(id, price);
            if (!success)
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(int id)
        {
            var success = await _priceService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
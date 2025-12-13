using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mobify.Data;

namespace Mobify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController(AppDbContext appDbContext) : ControllerBase
    {

        [HttpPost("addSeller")]
        public async Task<IActionResult> addNewSellers([FromBody] Sellers model)
        {
            appDbContext.Sellers.Add(model);
            await appDbContext.SaveChangesAsync();

            return Ok(model);

        }

        [HttpGet("GetAllSellers")]
        public async Task<IActionResult> GetAllSellers()
        {
            var sellers = await appDbContext.Sellers.ToListAsync();
            return Ok(sellers);
        }

        [HttpGet("{sellerId}")]
        public async Task<IActionResult> GetSellerById(int sellerId)
        {
            var seller = await appDbContext.Sellers
                .FirstOrDefaultAsync(s => s.SellerId == sellerId);

            if (seller == null)
            {
                return NotFound($"Seller with ID {sellerId} not found.");
            }

            return Ok(seller);
        }

        [HttpPut("{sellerId}")]
        public async Task<IActionResult> UpdateSeller(int sellerId, [FromBody] Sellers model)
        {
            if (model == null || sellerId != model.SellerId)
            {
                return BadRequest("Invalid seller data.");
            }

            var seller = await appDbContext.Sellers.FirstOrDefaultAsync(s => s.SellerId == sellerId);
            if (seller == null)
            {
                return NotFound($"Seller with ID {sellerId} not found.");
            }

            // Update fields
            seller.SellerName = model.SellerName;
            seller.StoreName = model.StoreName;
            seller.Email = model.Email;
            seller.PhoneNo = model.PhoneNo;
            seller.State = model.State;
            seller.City = model.City;
            seller.Address = model.Address;

            await appDbContext.SaveChangesAsync();

            return Ok(seller);
        }

    }
}

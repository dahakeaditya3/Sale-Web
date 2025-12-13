using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mobify.Data;

namespace Mobify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpPost("addProducts")]
        public async Task<IActionResult> addNewProducts([FromBody] Products model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            appDbContext.Products.Add(model);
            await appDbContext.SaveChangesAsync();

            return Ok(model);

        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await appDbContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("GetProductsBySeller/{sellerId}")]
        public async Task<IActionResult> GetProductsBySeller(int sellerId)
        {
            var products = await appDbContext.Products
                .Where(p => p.SellerId == sellerId)
                .ToListAsync();

            if (products == null || products.Count == 0)
                return NotFound("No products found for this seller.");

            return Ok(products);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            appDbContext.Products.Remove(product);
            await appDbContext.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully." });
        }

   

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await appDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
                return NotFound();
            return Ok(product); // ✅ returns single product, not array
        }
    }
}

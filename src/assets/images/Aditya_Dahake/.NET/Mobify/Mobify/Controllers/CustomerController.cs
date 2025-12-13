using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mobify.Data;

namespace Mobify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CustomerController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddNewCustomer([FromBody] Customers model)
        {
            _appDbContext.Customers.Add(model);
            await _appDbContext.SaveChangesAsync();

            return Ok(model);

        }

        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _appDbContext.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("GetCustomerById/{customerId}")]
        public async Task<IActionResult> GetProductById(int customerId)
        {
            var customer = await _appDbContext.Customers.FirstOrDefaultAsync(p => p.CustomerId == customerId);
            if (customer == null)
                return NotFound();
            return Ok(customer); 
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateSeller(int customerId, [FromBody] Customers model)
        {
            if (model == null || customerId != model.CustomerId)
            {
                return BadRequest("Invalid seller data.");
            }

            var customer = await _appDbContext.Customers.FirstOrDefaultAsync(s => s.CustomerId == customerId);
            if (customer == null)
            {
                return NotFound($"Customer with ID {customerId} not found.");
            }

            // Update fields
            customer.CustomerName = model.CustomerName;
            customer.Email = model.Email;
            customer.PhoneNo = model.PhoneNo;
            customer.City = model.City;
            customer.Gender = model.Gender;

            await _appDbContext.SaveChangesAsync();

            return Ok(customer);
        }
    }
}

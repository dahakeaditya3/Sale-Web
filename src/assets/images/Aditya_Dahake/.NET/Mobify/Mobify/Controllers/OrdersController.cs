using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mobify.Data;

namespace Mobify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController (AppDbContext appDbContext) : ControllerBase
    {

        [HttpPost("addOrder")]
        public async Task<IActionResult> addNewOrders([FromBody] Orders model)
        {
            appDbContext.Orders.Add(model);
            await appDbContext.SaveChangesAsync();

            return Ok(model);

        }

    }

}

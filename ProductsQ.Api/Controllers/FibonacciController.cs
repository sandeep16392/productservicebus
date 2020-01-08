using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductsQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FibonacciController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get([FromQuery]int n)
        {
            int a = 0, b = 1, sum=0, i;
            for (i = 1; i < n; ++i)  
            {
                sum = a + b;
                a = b;
                b = sum;
            }

            return Ok(sum);
        }
    }
}
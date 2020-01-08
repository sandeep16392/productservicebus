using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// <summary>
        /// Returns the nth number in the fibonacci sequence.
        /// </summary>
        /// <param name="n">The index (n) of the fibonacci sequence</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([Required, FromQuery]int n)
        {
            if (n == 0)
                return Ok(0);
            if (n == 1)
                return Ok(1);
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
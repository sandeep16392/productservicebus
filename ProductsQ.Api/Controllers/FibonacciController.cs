using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductsQ.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class FibonacciController : ControllerBase
    {
        [HttpGet("Fibonacci")]
        public IActionResult Get([Required, FromQuery]long n)
        {
            int a = 0, b = 1, sum = 0, i;
            for (i = 1; i < n; ++i)
            {
                sum = a + b;
                a = b;
                b = sum;
            }

            return Ok(sum);
        }

        [HttpGet("Token")]
        public IActionResult GetToken()
        {
            return Ok("57c331c1-ee82-47d9-b564-9f2ffa2fbbb1");
        }

        [HttpGet("ReverseWords")]
        public IActionResult Reverse([FromQuery]string sentence)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return Ok(string.Empty);
            }
            var strArr = sentence.Split(' ');

            for (var i = 0; i < strArr.Length; i++)
            {
                strArr[i] = ReverseString(strArr[i]);
            }
            var result = string.Join(' ', strArr);
            return Ok(result);
        }

        [HttpGet("TriangleType")]
        public IActionResult TriangleType([Required, FromQuery]int a, [Required, FromQuery]int b, [Required, FromQuery]int c)
        {
            int s1 = a;
            int s2 = b;
            int s3 = c;

            int[] values = new int[3] { s1, s2, s3 };

            if (s1 <= 0 || s2 <= 0 || s3 <= 0)
            {
                return Ok("Error");
            }
            else if (values.Distinct().Count() == 1)
            {
                return Ok("Equilateral");
            }
            else if (values.Distinct().Count() == 2)
            {
                return Ok("Isosceles");
            }
            else if (values.Distinct().Count() == 3)
            {
                return Ok("Scalene");
            }
            else
            {
                return Ok("Error");
            }
        }

        private string ReverseString(string str)
        {
            var result = "";
            for (var i = str.Length - 1; i >= 0; i--)
            {
                result = result + str[i];
            }

            return result;
        }
    }
}
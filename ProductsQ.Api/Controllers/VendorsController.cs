using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Products.DAL.Abstraction;
using Products.DAL.DomainModels;

namespace ProductsQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorRepository _vendorRepository;
        public VendorsController(IVendorRepository vendorRepository)
        {
            this._vendorRepository = vendorRepository;
            
        }
        /// <summary>
        /// Adds Vendors to repository.
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody][Required] VendorDm vendor)
        {
            try
            {
                var isCreated = await _vendorRepository.Add(vendor);
                if (isCreated)
                    return Ok(vendor);
                else
                {
                    return Ok("Some error occurred.");
                }
            }
            catch (ArgumentException)
            {

                return Conflict("Vendor Already Present.");
            }
        }
        [HttpDelete("/{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var isDeleted = await _vendorRepository.Delete(code);
                if (isDeleted)
                    return Ok("Vendor Deleted!");
                else
                {
                    return Ok("Some error occured");
                }
            }
            catch (ArgumentNullException)
            {

                return Conflict("Vendor Not Present.");
            }
        }
        /// <summary>
        /// Retrieve all vendors present in repository
        /// </summary>
        /// <returns></returns>
        [HttpGet("/vendors")]
        public async Task<IActionResult> RetrieveAll()
        {
            var vendor = await _vendorRepository.RetrieveAll();
            if (vendor == null)
                return NotFound("Not Vendor present!");
            else
            {
                return Ok(vendor);
            }
        }
        [HttpGet("vendor/{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var vendor = await _vendorRepository.Get(code);
            if (vendor == null)
                return NotFound("Vendor not found!");
            else
            {
                return Ok(vendor);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.DAL.Abstraction;
using Products.DAL.DomainModels;
using Products.ServiceBusMessaging;

namespace ProductsQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayloadController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly ServiceBusSender _serviceBusSender;

        public PayloadController(IProductsRepository productsRepository, IVendorRepository vendorRepository, ServiceBusSender serviceBusSender)
        {
            this._productsRepository = productsRepository;
            this._vendorRepository = vendorRepository;
            this._serviceBusSender = serviceBusSender;
        }
        /// <summary>
        /// This method queues up a product in azure message sevice bus. Then the receiver kicks in and add the product from queue to DB.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody][Required] ProductsPayloadDm request)
        {
            //try
            //{
            //    //var response = await _vendorRepository.Get(request.Vendor.Code);
            //    if(response == null)
            //    {
            //        throw new ArgumentNullException("Vendor not present.");
            //    }
            //}
            //catch (ArgumentException)
            //{
            //    return BadRequest($"Vendor with code{request.Vendor.Code} is not valid.");
            //}

            // Send this to the bus for the other services
            await _serviceBusSender.SendMessage(request);

            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productsRepository.RetrieveAll();
            if (products == null)
                return NotFound("No Product found!");
            else
            {
                return Ok(products);
            }
        }
    }
}
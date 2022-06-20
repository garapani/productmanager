using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using ProductManager.Domain.Constansts;
using ProductManager.Domain.Entities;
using ProductManager.Features.Products.Commands;
using ProductManager.Features.Products.Queries;
using ProductManager.GenericResponse;
using ProductManager.ViewModel;

namespace ProductManager.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductController : BaseApiController
    {
        private IMapper _mapper;

        public ProductController(IMapper mapper)
        {
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<Response<ProductViewModel>> PostProduct(CreateProductViewModel product)
        {
            var createProductCommand = this._mapper.Map<CreateProductViewModel, CreateProductCommand>(product);
            var createdProduct= await this.Mediator.Send(createProductCommand);
            if(createdProduct!=null && createdProduct.ResultCode == ResultCode.Success)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
                return createdProduct;
            }
            else
            {
                return createdProduct;
            }
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<Response<ProductViewModel>> GetProduct(int id, [FromQuery] CurrencyType? currency = CurrencyType.USD)
        {
            return await this.Mediator.Send(new GetProductByIdQuery() { Id = id, Currency = currency });
        }

        [HttpGet("")]
        public async Task<Response<ProductViewModel>> GetProductByName([FromQuery] string name = "", [FromQuery] CurrencyType? currency = CurrencyType.USD)
        {
            return await this.Mediator.Send(new GetProductByNameQuery() { Name = name, Currency = currency });
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<Response<ProductViewModel>> DeleteProductById(int id)
        {
            return await this.Mediator.Send(new DeleteProductByIdCommand() { Id = id });
        }

        [HttpDelete("")]
        public async Task<Response<ProductViewModel>> DeleteProductByName([FromQuery] string name = "")
        {
            return await this.Mediator.Send(new DeleteProductByNameCommand() { Name = name });
        }

        [HttpGet("mostviewed")]
        public async Task<Response<ICollection<ProductViewModel>>> GetMostViewedProducts([FromQuery] int count = 5, [FromQuery] CurrencyType? currency = CurrencyType.USD)
        {
            return await this.Mediator.Send(new GetMostViewedProductsQuery() { Count = count, Currency = currency });
        }
    }
}

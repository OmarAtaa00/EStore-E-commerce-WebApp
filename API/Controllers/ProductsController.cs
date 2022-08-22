using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Entities;
using Core.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        //Replacing the easy service and adding more complex one but it will help 
        //later to solve more problem to come TRUST

        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductType> productTypeRepo,
        IGenericRepository<ProductBrand> productBrandRepo
        )
        {
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;



        }
        // Getting Products from database
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productsRepo.ListAllAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            return await _productsRepo.GetByIdAsync(id);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrandsAsync()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypesAsync()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }

    }
}
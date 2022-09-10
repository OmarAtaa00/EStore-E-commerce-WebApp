using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{

    public class ProductsController : BaseController
    {
        //Replacing the easy service and adding more complex one but it will help 
        //later to solve more problem to come TRUST

        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductType> productTypeRepo,
        IGenericRepository<ProductBrand> productBrandRepo,
        IMapper mapper
        )
        {
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;


        }
        // Getting Products from database
        [HttpGet]
        public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new TypesAndBrandsSpec(productParams); //Start here  while debugging 

            var countSpec = new FiltersCountSpec(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);


            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] //Document it for swagger 
        public async Task<ActionResult<ProductToReturnDto>> GetProducts(int id)
        {
            var spec = new TypesAndBrandsSpec(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend_API.DTO;
using Backend_API.Entities;
using Backend_API.Errors;
using Backend_API.Helpers;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers
{

    public class ProductsController : BaseAPIController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery]ProductSpecificationParams productParams)
        {
            ISpecification<Product> specification = new ProductsWithTypesAndBrandsSpecification(productParams);
            ISpecification<Product> countSpecification = new ProductWithFiltersForCountSpecification(productParams);
            int totalItems = await _productRepository.CountAsync(countSpecification);
            IReadOnlyList<Product> result = await _productRepository.ListAllAsync(specification);
            IReadOnlyList<ProductDTO> data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(result);
            return Ok(new Pagination<ProductDTO>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO?>> GetProduct(int id)
        {
            ISpecification<Product> specification = new ProductsWithTypesAndBrandsSpecification(id);
            Product? result = await _productRepository.GetEntityWithSpecification(specification);
            if (result is null)
            {
                return NotFound(new APIResponse(404));
            }
            return Ok(_mapper.Map<Product,ProductDTO>(result));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepository.GetAllAsync());
        }
        
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepository.GetAllAsync());
        }
    }
}

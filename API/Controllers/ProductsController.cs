using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IGenericRepository<ProductBrand> _productBrandRepo;
		private readonly IGenericRepository<ProductType> _productTypeRepo;
		private readonly IMapper _mapper;

		public ProductsController(IGenericRepository<Product> productRepo,
			IGenericRepository<ProductBrand> productBrandRepo,
			IGenericRepository<ProductType> productTypeRepo,
			IMapper mapper)
		{
			_productRepo = productRepo;
			_productBrandRepo = productBrandRepo;
			_productTypeRepo = productTypeRepo;
			_mapper = mapper;
		}

		[HttpGet]
		//[Cached(300)]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
		[FromQuery] ProductSpecificationParams specParams)
		{
			var specification = new ProductsWithBrandsAndTypesSpecifications(specParams);
			var countSpec = new ProductWithFiltersForCountSpecification(specParams);

			var totalItems = await _productRepo.CountAsync(countSpec);
			var products = await _productRepo.GetListAllWithSpecAsync(specification);

			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data));
		}

		[HttpGet("{id}")]
		//[Cached(300)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var specification = new ProductsWithBrandsAndTypesSpecifications(id);
			var product = await _productRepo.GetEntityWithSpecAsync(specification);

			if (product == null)
				return NotFound(new ApiResponse(404));

			return _mapper.Map<Product, ProductToReturnDto>(product);
		}

		[HttpGet("brands")]
		//[Cached(300)]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
		{
			return Ok(await _productBrandRepo.GetListAllAsync());
		}

		[HttpGet("types")]
		//[Cached(300)]
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
		{
			return Ok(await _productTypeRepo.GetListAllAsync());
		}
	}
}
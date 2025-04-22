using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using X.PagedList;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("products/{id:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int id)
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
            var products = await _unitOfWork.ProductRepository.GetProductsByCategoryAsync(id);
            if (products is null)
                return NotFound("Products not found");

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync(productsParameters);

            if (products is null)
                return NotFound("Products not found");

            return GetProducts(products);
        }

        [HttpGet("filter/price/pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsFilterPrice productsFilterPrice)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsFilterPriceAsync(productsFilterPrice);

            return GetProducts(products);
        }

        private ActionResult<IEnumerable<ProductDTO>> GetProducts(IPagedList<Product> products)
        {
            var metadata = new
            {
                products.Count,
                products.PageSize,
                products.PageCount,
                products.TotalItemCount,
                products.HasNextPage,
                products.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(_mapper.Map<IEnumerable<ProductDTO>>(products));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            if (products is null)
                return NotFound("Products not found");

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")] // url/api/products/1
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);
            if (product is null)
                return NotFound("Product not found");

            var productDTO = _mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDTO)
        {
            if (productDTO is null)
                return BadRequest("Product is null");

            var product = _mapper.Map<Product>(productDTO);

            var newProduct = _unitOfWork.ProductRepository.Create(product);
            await _unitOfWork.CommitAsync();

            var newProductDTO = _mapper.Map<ProductDTO>(newProduct);

            return CreatedAtRoute("GetProduct", new { id = newProductDTO.ProductId }, newProductDTO);
        }

        [HttpPost("list")]
        public async Task<ActionResult> Post(List<ProductDTO> productsDTO)
        {
            if (productsDTO is null)
                return BadRequest("Products is null");
            var products = _mapper.Map<List<Product>>(productsDTO);
            foreach (var product in products)
            {
                _unitOfWork.ProductRepository.Create(product);
            }
            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpPatch("{id}/PartialUpdate")]
        public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(
            int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
        {
            if (patchProductDTO is null || id <= 0)
                return BadRequest();

            var product = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);
            if (product is null)
                return NotFound();

            var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchProductDTO.ApplyTo(productUpdateRequest, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            if (!ModelState.IsValid || !TryValidateModel(productUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(productUpdateRequest, product);

            var productUpdated = _unitOfWork.ProductRepository.Update(product);

            await _unitOfWork.CommitAsync();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(productUpdated));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
                return BadRequest();

            var product = _mapper.Map<Product>(productDTO);

            var result = _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.CommitAsync();

            var updatedProductDTO = _mapper.Map<ProductDTO>(result);

            return Ok(updatedProductDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product is null)
                return NotFound();

            var deletedProduct = _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.CommitAsync();

            var deletedProductDTO = _mapper.Map<ProductDTO>(deletedProduct);

            return Ok(deletedProductDTO);
        }
    }
}
using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<ProductDTO>> GetProductsByCategory(int id)
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
            var products = _unitOfWork.ProductRepository.GetProductsByCategory(id);
            if (products is null)
                return NotFound("Products not found");

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProductDTO>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = _unitOfWork.ProductRepository.GetProducts(productsParameters);
            if (products is null)
                return NotFound("Products not found");
            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            var products = _unitOfWork.ProductRepository.GetAll();
            if (products is null)
                return NotFound("Products not found");

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")] // url/api/products/1
        public ActionResult<Product> Get(int id)
        {
            var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);
            if (product is null)
                return NotFound("Product not found");

            var productDTO = _mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpPost]
        public ActionResult<ProductDTO> Post(ProductDTO productDTO)
        {
            if (productDTO is null)
                return BadRequest("Product is null");

            var product = _mapper.Map<Product>(productDTO);

            var newProduct = _unitOfWork.ProductRepository.Create(product);
            _unitOfWork.Commit();

            var newProductDTO = _mapper.Map<ProductDTO>(newProduct);

            return CreatedAtRoute("GetProduct", new { id = newProductDTO.ProductId }, newProductDTO);
        }

        [HttpPost("list")]
        public ActionResult Post(List<ProductDTO> productsDTO)
        {
            if (productsDTO is null)
                return BadRequest("Products is null");
            var products = _mapper.Map<List<Product>>(productsDTO);
            foreach (var product in products)
            {
                _unitOfWork.ProductRepository.Create(product);
            }
            _unitOfWork.Commit();
            return Ok();
        }

        [HttpPatch("{id}/PartialUpdate")]
        public ActionResult<ProductDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
        {
            if (patchProductDTO is null || id <= 0)
                return BadRequest();

            var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);
            if (product is null)
                return NotFound();

            var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchProductDTO.ApplyTo(productUpdateRequest, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            if (!ModelState.IsValid || !TryValidateModel(productUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(productUpdateRequest, product);

            var productUpdated = _unitOfWork.ProductRepository.Update(product);

            _unitOfWork.Commit();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(productUpdated));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProductDTO> Put(int id, ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
                return BadRequest();

            var product = _mapper.Map<Product>(productDTO);

            var result = _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();

            var updatedProductDTO = _mapper.Map<ProductDTO>(result);

            return Ok(updatedProductDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProductDTO> Delete(int id)
        {
            var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);

            if (product is null)
                return NotFound();

            var deletedProduct = _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();

            var deletedProductDTO = _mapper.Map<ProductDTO>(deletedProduct);

            return Ok(deletedProductDTO);
        }
    }
}
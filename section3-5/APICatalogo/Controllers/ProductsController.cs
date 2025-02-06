using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IRepository<Product> _repository;

        public ProductsController(IRepository<Product> repository, IProductRepository productRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
        }

        [HttpGet("products/{id:int}")]
        public ActionResult<IEnumerable<Product>> GetProductsByCategory(int id)
        {
            var products = _productRepository.GetProductsByCategory(id);
            if (products is null)
                return NotFound("Products not found");
            return Ok(products);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _repository.GetAll();
            if (products is null)
                return NotFound("Products not found");

            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")] // url/api/products/1
        public ActionResult<Product> Get(int id)
        {
            var product = _repository.Get(p => p.ProductId == id);
            if (product is null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
                return BadRequest("Product is null");

            var newProduct = _repository.Create(product);
            return CreatedAtRoute("GetProduct", new { id = newProduct.ProductId }, newProduct);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            var result = _repository.Update(product);

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _repository.Get(p => p.ProductId == id);

            if (product is null)
                return NotFound();

            var deletedProduct = _repository.Delete(product);

            return Ok(deletedProduct);
        }
    }
}
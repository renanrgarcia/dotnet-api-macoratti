using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _repository.GetProducts().ToList();
            if (products is null)
                return NotFound("Products not found");

            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")] // url/api/products/1
        public ActionResult<Product> Get(int id)
        {
            var product = _repository.GetProduct(id);
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

            bool result = _repository.Update(product);

            if (result)
                return Ok(product);
            else
                return StatusCode(500, "Error while updating product");
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            if (_repository.GetProduct(id) is null)
                return NotFound();

            bool result = _repository.Delete(id);

            if (result)
                return Ok($"Product with id = {id} deleted");
            else
                return StatusCode(500, $"Error while deleting product with id = {id}");
        }
    }
}
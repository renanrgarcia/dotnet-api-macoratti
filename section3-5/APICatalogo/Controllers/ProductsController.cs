using ApiCatalogo.Models;
using ApiCatalogo.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("products/{id:int}")]
        public ActionResult<IEnumerable<Product>> GetProductsByCategory(int id)
        {
            var products = _unitOfWork.ProductRepository.GetProductsByCategory(id);
            if (products is null)
                return NotFound("Products not found");
            return Ok(products);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _unitOfWork.ProductRepository.GetAll();
            if (products is null)
                return NotFound("Products not found");

            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")] // url/api/products/1
        public ActionResult<Product> Get(int id)
        {
            var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);
            if (product is null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
                return BadRequest("Product is null");

            var newProduct = _unitOfWork.ProductRepository.Create(product);
            _unitOfWork.Commit();

            return CreatedAtRoute("GetProduct", new { id = newProduct.ProductId }, newProduct);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            var result = _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);

            if (product is null)
                return NotFound();

            var deletedProduct = _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();

            return Ok(deletedProduct);
        }
    }
}
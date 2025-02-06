using ApiCatalogo.Models;
using ApiCatalogo.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriesController(IConfiguration configuration,
                                    ILogger<CategoriesController> logger,
                                    IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<Category> Get(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == id);

            if (category is null)
            {
                _logger.LogInformation($"Category id {id} not found.");
                return NotFound($"Category id {id} not found.");
            }

            return Ok(category);
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                _logger.LogInformation("Category object sent from client is null.");
                return BadRequest("Category object sent from client is null.");
            }

            var createdCategory = _unitOfWork.CategoryRepository.Create(category);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("GetCategory", new
            {
                id = createdCategory.CategoryId
            }, createdCategory);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                _logger.LogInformation($"Category id {id} doesn't match with category id {category.CategoryId}.");
                return BadRequest($"Category id {id} doesn't match with category id {category.CategoryId}.");
            }

            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Commit();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == id);

            if (category is null)
            {
                _logger.LogWarning($"Category id {id} not found.");
                return NotFound($"Category id {id} not found.");
            }

            var deletedCategory = _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();

            return Ok(deletedCategory);
        }

        // [HttpGet("UsingFromServices/{name}")]
        // public ActionResult<string> GetGreetingsFromServices([FromServices] IMyService myService, string name)
        // {
        //     return myService.Greeting(name);
        // }

        // [HttpGet("NotUsingFromServices/{name}")]
        // public ActionResult<string> GetGreetingsWithoutFromServices(IMyService myService, string name)
        // {
        //     return myService.Greeting(name);
        // }

        // [HttpGet("ReadConfigurationFile")]
        // public string GetConfiguration()
        // {
        //     var value1 = _configuration["Key1"];
        //     var value2 = _configuration["Key2"];
        //     var section1 = _configuration["Section1:Key1"];

        //     return $"Key1: {value1} | Key2: {value2} | Section1 => Key1: {section1}";
        // }

        // [HttpGet("products")]
        // public IEnumerable<Category> GetCategoriesProducts()
        // {
        //     // return _context.Categories
        //     //     .Include(c => c.Products)
        //     //     .ToList();
        //     _logger.LogInformation("=============== GET api/categories/products ===============");

        //     return _context.Categories
        //         .Include(c => c.Products)
        //         .Where(c => c.CategoryId <= 5)
        //         .ToList();
        // }
    }
}
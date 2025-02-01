using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriesController(AppDbContext context,
                                    IConfiguration configuration,
                                    ILogger<CategoriesController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
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

        [HttpGet("ReadConfigurationFile")]
        public ActionResult<string> GetConfiguration()
        {
            var value1 = _configuration["Key1"];
            var value2 = _configuration["Key2"];
            var section1 = _configuration["Section1:Key1"];

            return $"Key1: {value1} | Key2: {value2} | Section1 => Key1: {section1}";
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
        {
            // return _context.Categories
            //     .Include(c => c.Products)
            //     .ToList();
            _logger.LogInformation("=============== GET api/categories/products ===============");

            return _context.Categories
                .Include(c => c.Products)
                .Where(c => c.CategoryId <= 5)
                .ToList();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> Get()
        {
            try
            {
                return _context.Categories.AsNoTracking().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error while getting the categories from the database.");
            }
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<Category> Get(int id)
        {
            // throw new Exception("Exception while fetching a category by id.");
            //string[] teste = null;
            //if (teste.Length > 0)
            //{
            //    // Do something
            //}

            var category = _context.Categories?.FirstOrDefault(p => p.CategoryId == id);

            _logger.LogInformation($"=============== GET api/categories/id = {id} ===============");

            if (category is null)
                _logger.LogInformation($"Category id {id} not found.");
            return NotFound("Category not found.");
            return category;
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
                return BadRequest();
            _context.Categories?.Add(category);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetCategory", new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId)
                return BadRequest();
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Category> Delete(int id)
        {
            var category = _context.Categories?.FirstOrDefault(p => p.CategoryId == id);
            if (category is null)
                return NotFound("Category not found.");
            _context.Categories?.Remove(category);
            _context.SaveChanges();
            return Ok();
        }
    }
}
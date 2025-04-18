using ApiCatalogo.DTOs;
using ApiCatalogo.Extensions;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public ActionResult<IEnumerable<CategoryDTO>> Get()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();

            if (categories == null)
                return NotFound("There are no categories.");

            var categoriesDTO = categories.ToCategoryDTOList();

            return Ok(categoriesDTO);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<CategoryDTO> Get(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == id);

            if (category is null)
            {
                _logger.LogInformation($"Category id {id} not found.");
                return NotFound($"Category id {id} not found.");
            }

            var categoryDTO = category.ToCategoryDTO();

            return Ok(categoryDTO);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoryDTO>> Get([FromQuery] CategoriesParameters categoriesParameters)
        {
            var categories = _unitOfWork.CategoryRepository.GetCategories(categoriesParameters);

            if (categories == null)
                return NotFound("There are no categories.");

            return GetCategories(categories);
        }

        [HttpGet("filter/name/pagination")]
        public ActionResult<IEnumerable<CategoryDTO>> Get([FromQuery] CategoriesFilterName categoriesFilterName)
        {
            var categories = _unitOfWork.CategoryRepository.GetCategoriesFilterName(categoriesFilterName);

            return GetCategories(categories);
        }

        private ActionResult<IEnumerable<CategoryDTO>> GetCategories(PagedList<Category> categories)
        {
            var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.TotalPages,
                categories.HasNext,
                categories.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(categories.ToCategoryDTOList());
        }

        [HttpPost]
        public ActionResult<CategoryDTO> Post(CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                _logger.LogInformation("Category object sent from client is null.");
                return BadRequest("Category object sent from client is null.");
            }

            var category = categoryDTO.ToCategory();

            var createdCategory = _unitOfWork.CategoryRepository.Create(category);
            _unitOfWork.Commit();

            var newCategoryDTO = createdCategory.ToCategoryDTO();

            return new CreatedAtRouteResult("GetCategory", new
            {
                id = newCategoryDTO.CategoryId
            }, newCategoryDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
            {
                _logger.LogInformation($"Category id {id} doesn't match with category id {categoryDTO.CategoryId}.");
                return BadRequest($"Category id {id} doesn't match with category id {categoryDTO.CategoryId}.");
            }

            var category = categoryDTO.ToCategory();

            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Commit();

            var updatedCategoryDTO = category.ToCategoryDTO();

            return Ok(updatedCategoryDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoryDTO> Delete(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == id);

            if (category is null)
            {
                _logger.LogWarning($"Category id {id} not found.");
                return NotFound($"Category id {id} not found.");
            }

            var deletedCategory = _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();

            var deletedCategoryDTO = deletedCategory.ToCategoryDTO();

            return Ok(deletedCategoryDTO);
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
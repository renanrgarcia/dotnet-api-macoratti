using ApiCatalogo.DTOs;
using ApiCatalogo.Extensions;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using X.PagedList;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CategoriesController(ILogger<CategoriesController> logger,
                                    IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            if (categories == null)
                return NotFound("There are no categories.");

            var categoriesDTO = categories.ToCategoryDTOList();

            return Ok(categoriesDTO);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.CategoryId == id);

            if (category is null)
            {
                _logger.LogInformation($"Category id {id} not found.");
                return NotFound($"Category id {id} not found.");
            }

            var categoryDTO = category.ToCategoryDTO();

            return Ok(categoryDTO);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParameters)
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync(categoriesParameters);

            if (categories == null)
                return NotFound("There are no categories.");

            return GetCategories(categories);
        }

        [HttpGet("filter/name/pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesFilterName categoriesFilterName)
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesFilterNameAsync(categoriesFilterName);

            return GetCategories(categories);
        }

        private ActionResult<IEnumerable<CategoryDTO>> GetCategories(IPagedList<Category> categories)
        {
            var metadata = new
            {
                categories.Count,
                categories.PageSize,
                categories.PageCount,
                categories.TotalItemCount,
                categories.HasNextPage,
                categories.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(categories.ToCategoryDTOList());
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                _logger.LogInformation("Category object sent from client is null.");
                return BadRequest("Category object sent from client is null.");
            }

            var category = categoryDTO.ToCategory();

            var createdCategory = _unitOfWork.CategoryRepository.Create(category);
            await _unitOfWork.CommitAsync();

            var newCategoryDTO = createdCategory.ToCategoryDTO();

            return new CreatedAtRouteResult("GetCategory", new
            {
                id = newCategoryDTO.CategoryId
            }, newCategoryDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
            {
                _logger.LogInformation($"Category id {id} doesn't match with category id {categoryDTO.CategoryId}.");
                return BadRequest($"Category id {id} doesn't match with category id {categoryDTO.CategoryId}.");
            }

            var category = categoryDTO.ToCategory();

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.CommitAsync();

            var updatedCategoryDTO = category.ToCategoryDTO();

            return Ok(updatedCategoryDTO);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.CategoryId == id);

            if (category is null)
            {
                _logger.LogWarning($"Category id {id} not found.");
                return NotFound($"Category id {id} not found.");
            }

            var deletedCategory = _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.CommitAsync();

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
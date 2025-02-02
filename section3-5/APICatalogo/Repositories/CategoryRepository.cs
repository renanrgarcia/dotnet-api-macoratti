using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
        }


        public Category CreateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _context.Entry(category).State = EntityState.Modified; // Shows that the entity is being modified and will be updated in the database
            _context.SaveChanges();
            return category;
        }

        public Category DeleteCategory(int categoryId)
        {
            var category = _context.Categories.Find(categoryId); // Optimized to search Id

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return category;
        }


    }
}
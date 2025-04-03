using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext context;

        public CategoryRepository(DataContext context)
        {
            this.context = context;
        }
        public bool CategoryExists(int id)
        {
            return context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            context.Categories.Add(category);
            return Save();
        }
        public bool DeleteCategory(Category category)
        {
            context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return context.PokemonCategories.Where(p=>p.CategoryId==categoryId).Select(p => p.Pokemon).ToList();
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            context.Categories.Update(category);
            return Save();
        }
    }
}

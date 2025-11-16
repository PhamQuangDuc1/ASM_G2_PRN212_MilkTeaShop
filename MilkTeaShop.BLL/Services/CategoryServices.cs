using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class CategoryServices
    {
        private CategoryRepo _repo = new();

        public List<Category> GetAllServices()
        {
            return _repo.GetAll();
        }
        private CategoryRepo _categoryRepo = new();
        public List<Category> GetAllCategories()
        {
            try
            {

                return _categoryRepo.GetAll().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);

            }
        }
        public Category? GetCategoryById(int id)
        {
            try
            {
                var category = _categoryRepo.GetById(id);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the category with ID {id}.", ex);
            }
        }
        public Category CreateCategory(Category category)
        {
            try
            {
                return _categoryRepo.Add(category);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category.", ex);
            }

        }
        public Category UpdateCategory(Category category)
        {
            try
            {
                return _categoryRepo.Update(category);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }
        public bool DeleteCategory(int id)
        {
            try
            {
                return _categoryRepo.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the category with ID {id}.", ex);
            }
        }
    }
}

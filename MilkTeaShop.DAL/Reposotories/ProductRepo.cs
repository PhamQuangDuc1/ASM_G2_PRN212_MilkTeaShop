using Microsoft.EntityFrameworkCore;
using MilkTeaShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.DAL.Reposotories
{
    public class ProductRepo
    {
        private MilkTeaShopDbContext _dbContext;

        public Product Add(Product product)
        {
            _dbContext = new();
            var entity = _dbContext.Products.Add(product).Entity;
            _dbContext.SaveChanges();  
            return entity;
        }

        

        public bool Delete(int id)
        {
            try 
            {
                _dbContext = new();
                var product = _dbContext.Products.Find(id);
                if (product == null) return false;
                
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                return true;
            } catch (Exception ex) 
            {
                throw new Exception($"Error deleting product: {ex.Message}", ex);
            }
            
        }

        public List<Product> GetActive()
        {
            _dbContext = new();
            return _dbContext.Products.Where(p => p.IsActive == true).Include(p => p.Category).ToList();
        }

        public List<Product> GetAll()
        {
            _dbContext = new();
            return _dbContext.Products.ToList();
        }
        public void Update(Product x)
        {
            _dbContext = new();
            _dbContext.Products.Update(x);
            _dbContext.SaveChanges();
        }

        public List<Product> SearchProductsByName(string keyword)
        {
            return _dbContext.Products
                .Where(p => p.ProductName.Contains(keyword))
                .ToList();
        }

        private MilkTeaShopDbContext _context;
        



       
        

        

        public IEnumerable<Product> getAllActiveFalse()
        {
            _context = new();
            return _context.Products.Where(p => p.IsActive == false).Include("Category").ToList();
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            _context = new();
            return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public Product? GetById(int id)
        {
            try
            {
                _context = new();
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductPrices)
                    .Include(p => p.OrderDetails)
                    .Include(p => p.OrderDetailToppings)
                    .Include(p => p.Vouchers)
                    .FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting product by ID: {ex.Message}", ex);
            }
        }

        public bool SoftDelete(int id)
        {
            try
            {
                _context = new();
                var product = _context.Products.Find(id);
                if (product == null) return false;

                product.IsActive = false;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting product: {ex.Message}", ex);
            }
        }

        
    }
}

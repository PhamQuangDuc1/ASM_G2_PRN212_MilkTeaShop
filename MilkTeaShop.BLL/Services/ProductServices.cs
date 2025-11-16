using MilkTeaShop.DAL.Entities;
using MilkTeaShop.DAL.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaShop.BLL.Services
{
    public class ProductServices
    {
        private ProductRepo _repo = new();

        public List<Product> GetAllProduct()
        {
            return _repo.GetAll();
        }

        

        public List<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _repo.GetAll();
            return _repo.SearchProductsByName(keyword);
        }
        public List<Product> GetToppingList()
        {
            return _repo.GetAll()
                        .Where(p => p.CategoryId == 2)
                        .ToList();
        }
        private ProductRepo _productRepo = new();
        public bool ActivateProduct(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID sản phẩm không hợp lệ");

                var product = _productRepo.GetById(id);
                if (product == null)
                    return false;

                product.IsActive = true;
                _productRepo.Update(product);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kích hoạt sản phẩm: {ex.Message}", ex);
            }
        }

        public Product CreateProduct(Product product)
        {
            try
            {

                // Validation
                if (string.IsNullOrWhiteSpace(product.ProductName))
                    throw new ArgumentException("Tên sản phẩm không được để trống");

                if (product.CategoryId <= 0)
                    throw new ArgumentException("Vui lòng chọn danh mục");

                product.ProductName = product.ProductName.Trim();
                return _productRepo.Add(product);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo sản phẩm: {ex.Message}", ex);
            }
        }

        public bool DeactivateProduct(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID sản phẩm không hợp lệ");

                return _productRepo.SoftDelete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi vô hiệu hóa sản phẩm: {ex.Message}", ex);
            }
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID sản phẩm không hợp lệ");

                return _productRepo.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa sản phẩm: {ex.Message}", ex);
            }
        }

        public List<Product> GetActiveProducts()
        {
            try
            {
                return _productRepo.GetActive().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy sản phẩm đang hoạt động: {ex.Message}", ex);
            }
        }

        public List<Product> GetActiveProductsFalse()
        {
            try
            {
                return _productRepo.getAllActiveFalse().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy sản phẩm không hoạt động: {ex.Message}", ex);
            }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                return _productRepo.GetAll().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}", ex);
            }
        }

        public Product? GetProductById(int id)
        {
            try
            {
                return _productRepo.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy sản phẩm: {ex.Message}", ex);
            }
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            try
            {
                return _productRepo.GetByCategory(categoryId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy sản phẩm theo danh mục: {ex.Message}", ex);
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                if (product.Id <= 0)
                    throw new ArgumentException("ID sản phẩm không hợp lệ");

                if (string.IsNullOrWhiteSpace(product.ProductName))
                    throw new ArgumentException("Tên sản phẩm không được để trống");

                if (product.CategoryId <= 0)
                    throw new ArgumentException("Vui lòng chọn danh mục");

                product.ProductName = product.ProductName.Trim();
                 _productRepo.Update(product);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật sản phẩm: {ex.Message}", ex);
            }
        }
    }
}

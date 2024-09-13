
using Application.İnterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Product entity)
        {
            // Veritabanına yeni bir ürün ekleme
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            // Veritabanından ürünü silme
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new ArgumentException("Product not found", nameof(id));
            }

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // Veritabanından tüm ürünleri getirme
            return await _repository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            // Veritabanından ürünü ID ile getirme
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new ArgumentException("Product not found", nameof(id));
            }

            return product;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            // Kategori ID'ye göre ürünleri getirme
            return await _repository.GetProductsByCategoryAsync(categoryId);
        }

        public async Task UpdateAsync(Product entity)
        {
            // Veritabanında ürünü güncelleme
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingProduct = await _repository.GetByIdAsync(entity.ProductId);
            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found", nameof(entity.ProductId));
            }

            await _repository.UpdateAsync(entity);
        }
    }
}

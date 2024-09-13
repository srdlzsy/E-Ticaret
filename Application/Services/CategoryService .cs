
using Application.İnterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Category category)
        {
            // Veritabanına yeni bir kategori ekleme
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await _categoryRepository.AddAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            // Veritabanından kategoriyi silme
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new ArgumentException("Category not found", nameof(id));
            }

            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // Veritabanındaki tüm kategorileri alma
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            // Veritabanından kategoriyi alma
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new ArgumentException("Category not found", nameof(id));
            }
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            // Veritabanındaki kategoriyi güncelleme
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var existingCategory = await _categoryRepository.GetByIdAsync(category.CategoryId);
            if (existingCategory == null)
            {
                throw new ArgumentException("Category not found", nameof(category.CategoryId));
            }

            await _categoryRepository.UpdateAsync(category);
        }
    }
}

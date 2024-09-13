using Infrastructure.data;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.İnterfaces;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MınıDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(MınıDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Tüm verileri getirir
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Belirli bir ID'ye göre veriyi getirir
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Yeni veri ekler
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(); // Veriyi kaydeder
        }

        // Veriyi günceller
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(); // Güncellemeyi kaydeder
        }

        // Veriyi siler
        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(); // Silme işlemini kaydeder
            }
        }
    }
}

using Application.İnterfaces;
using Domain.Models;
using Infrastructure.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
 
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MınıDbContext context) : base(context) { }

        // Eğer kategorilere özel ek metodlar varsa buraya ekleyebilirsiniz
    }
}

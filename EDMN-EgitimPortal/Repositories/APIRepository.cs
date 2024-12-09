using Microsoft.EntityFrameworkCore;
using EgitimPortalFinal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgitimPortalFinal.Repositories
{
    public class APIRepository : IAPIRepository
    {
        private readonly AppDbContext _context;

        public APIRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(string? tag)
        {
            IQueryable<Course> coursesQuery = _context.Course;

            if (!string.IsNullOrEmpty(tag))
            {
                coursesQuery = coursesQuery.Where(c => c.Tag.Contains(tag));
            }

            return await coursesQuery.ToListAsync();
        }
    }
}

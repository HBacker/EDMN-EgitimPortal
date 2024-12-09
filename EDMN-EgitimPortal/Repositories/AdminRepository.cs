using EgitimPortalFinal.Models;
using Microsoft.EntityFrameworkCore;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _context;

    public AdminRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        return await _context.Course.OrderByDescending(c => c.Id).ToListAsync();
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        return await _context.Course.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCourseAsync(Course course)
    {
        _context.Course.Add(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCourseAsync(Course course)
    {
        _context.Course.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCourseAsync(int id)
    {
        var course = await _context.Course.FindAsync(id);
        if (course != null)
        {
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CourseExistsAsync(int id)
    {
        return await _context.Course.AnyAsync(c => c.Id == id);
    }
}

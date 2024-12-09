using EgitimPortalFinal.Models;
using Microsoft.EntityFrameworkCore;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllCoursesAsync(string? searchString = null)
    {
        var courses = _context.Course.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            courses = courses.Where(c => c.Title.Contains(searchString));
        }

        return await courses.OrderByDescending(c => c.Id).ToListAsync();
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        return await _context.Course.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Course>> GetCoursesByTagAsync(string tag)
    {
        return await _context.Course
                             .Where(c => c.Tag == tag)
                             .OrderByDescending(c => c.Id)
                             .ToListAsync();
    }

    public async Task<IEnumerable<Course>> SearchCoursesAsync(string keyword)
    {
        var courses = await _context.Course
            .Where(c => c.Title.Contains(keyword) || c.Description.Contains(keyword) || c.Tag.Contains(keyword))
            .ToListAsync();

        return courses;
    }

}

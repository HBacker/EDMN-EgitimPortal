using EgitimPortalFinal.Models;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllCoursesAsync(string? searchString = null);
    Task<Course?> GetCourseByIdAsync(int id);
    Task<IEnumerable<Course>> GetCoursesByTagAsync(string tag);
    Task<IEnumerable<Course>> SearchCoursesAsync(string keyword);

}

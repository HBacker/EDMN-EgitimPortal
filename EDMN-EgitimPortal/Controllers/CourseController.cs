using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

public class CourseController : Controller
{
    private readonly ICourseRepository _courseRepository;
    private readonly INotyfService _notify;

    public CourseController(ICourseRepository courseRepository, INotyfService notify)
    {
        _courseRepository = courseRepository;
        _notify = notify;
    }

    public async Task<IActionResult> Course(string searchString)
    {
        var courses = await _courseRepository.GetAllCoursesAsync(searchString);
        return View(courses);
    }

    public async Task<IActionResult> Wordpress()
    {
        var courses = await _courseRepository.GetCoursesByTagAsync("Wordpress");
        return View(courses);
    }

    public async Task<IActionResult> Programlama()
    {
        var courses = await _courseRepository.GetCoursesByTagAsync("Programlama");

        return View(courses);
    }

    public async Task<IActionResult> SEO()
    {
        var courses = await _courseRepository.GetCoursesByTagAsync("SEO");
        return View(courses);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _courseRepository.GetCourseByIdAsync(id.Value);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    public async Task<IActionResult> Search(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            _notify.Warning("Lütfen bir anahtar kelime girin.");
            return RedirectToAction("Course");
        }

        var courses = await _courseRepository.SearchCoursesAsync(keyword);

        if (!courses.Any())
        {
            _notify.Warning("Aradığınız kriterlere uygun kurs bulunamadı.");
        }

        return View(courses);
    }
}

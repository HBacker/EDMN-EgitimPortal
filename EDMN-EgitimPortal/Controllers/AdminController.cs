using AspNetCoreHero.ToastNotification.Abstractions;
using EgitimPortalFinal.Models;
using EgitimPortalFinal.Repositories;
using EgitimPortalFinal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly IAdminRepository _adminRepository;
    private readonly IFileProvider _fileProvider;
    private readonly INotyfService _notify;
    private readonly IAuthRepository _authRepository;

    public AdminController(IAdminRepository adminRepository, IFileProvider fileProvider, INotyfService notify)
    {
        _adminRepository = adminRepository;
        _fileProvider = fileProvider;
        _notify = notify;
       
    }

    public async Task<IActionResult> List()
    {
        var courses = await _adminRepository.GetAllCoursesAsync();
        return View(courses);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseModel newCourse)
    {
        try
        {
            var rootFolder = _fileProvider.GetDirectoryContents("wwwroot");
            var photoUrl = "-";

            if (newCourse.PhotoFile != null)
            {
                var filename = Path.GetFileName(newCourse.PhotoFile.FileName);
                var photoPath = Path.Combine(rootFolder.First(x => x.Name == "CoursePhotos").PhysicalPath, filename);
                using var stream = new FileStream(photoPath, FileMode.Create);
                newCourse.PhotoFile.CopyTo(stream);
                photoUrl = filename;
            }

            var course = new Course
            {
                Title = newCourse.Title,
                Content = newCourse.Content,
                Description = newCourse.Description,
                Tag = newCourse.Tag,
                PhotoUrl = photoUrl
            };

            await _adminRepository.AddCourseAsync(course);
            _notify.Success("Ders başarıyla eklendi.");
            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            _notify.Error($"Bir hata oluştu: {ex.Message}");
            return View(newCourse);
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _adminRepository.GetCourseByIdAsync(id.Value);
        if (course == null)
        {
            return NotFound();
        }

        var model = new CourseModel
        {
            Id = course.Id,
            Title = course.Title,
            Content = course.Content,
            Description = course.Description,
            Tag = course.Tag
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CourseModel updatedCourse)
    {
        try
        {
            var course = await _adminRepository.GetCourseByIdAsync(updatedCourse.Id);
            if (course == null)
            {
                return NotFound();
            }

            course.Title = updatedCourse.Title;
            course.Content = updatedCourse.Content;
            course.Description = updatedCourse.Description;
            course.Tag = updatedCourse.Tag;

            if (updatedCourse.PhotoFile != null)
            {
                var rootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                var filename = Path.GetFileName(updatedCourse.PhotoFile.FileName);
                var photoPath = Path.Combine(rootFolder.First(x => x.Name == "CoursePhotos").PhysicalPath, filename);
                using var stream = new FileStream(photoPath, FileMode.Create);
                updatedCourse.PhotoFile.CopyTo(stream);
                course.PhotoUrl = filename;
            }

            await _adminRepository.UpdateCourseAsync(course);
            return RedirectToAction(nameof(List));
        }
        catch (Exception ex)
        {
            _notify.Error($"Bir hata oluştu: {ex.Message}");
            return View(updatedCourse);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _adminRepository.GetCourseByIdAsync(id.Value);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _adminRepository.DeleteCourseAsync(id);
        return RedirectToAction(nameof(List));
    }
}

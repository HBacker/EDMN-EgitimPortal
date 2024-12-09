using Microsoft.AspNetCore.Mvc;
using EgitimPortalFinal.Models;
using EgitimPortalFinal.Repositories;

namespace EgitimPortalFinal.Controllers
{
    [Route("[controller]/v1/ListCourses")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IAPIRepository _apiRepository;

        public APIController(IAPIRepository apiRepository)
        {
            _apiRepository = apiRepository;
        }

        // GET: /api/v1/ListCourses?tag=<Keyword>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses([FromQuery] string? tag)
        {
            var courses = await _apiRepository.GetCoursesAsync(tag);
            return Ok(courses);
        }
    }
}

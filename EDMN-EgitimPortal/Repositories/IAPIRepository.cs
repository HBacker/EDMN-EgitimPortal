using EgitimPortalFinal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EgitimPortalFinal.Repositories
{
    public interface IAPIRepository
    {
        Task<IEnumerable<Course>> GetCoursesAsync(string? tag);
    }
}

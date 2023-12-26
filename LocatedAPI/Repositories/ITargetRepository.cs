using LocatedAPI.Models;

namespace LocatedAPI.Repositories
{
    public interface ITargetRepository
    {
        Task<List<Target>> GetAllTargetsAsync(int personId);
        Task<Target> GetTargetByIdAsync(int id, int personId);
        Task<int> CreateTargetAsync(Target target);
        Task<bool> DelTargetByIdAsync(int id, int personId);
    }

}

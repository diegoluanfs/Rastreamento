using LocatedAPI.Models;

namespace LocatedAPI.Repositories
{
    public interface IRouteRepository
    {
        Task<List<TargetRoute>> GetAllRoutesAsync(int personId);
        Task<TargetRoute> GetRouteByIdAsync(int id, int personId);
    }

}

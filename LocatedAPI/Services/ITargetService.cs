using LocatedAPI.Models;
using LocatedAPI.Models.DTO;

namespace LocatedAPI.Services
{
    public interface ITargetService
    {
        Task<List<Target>> GetAllTargetsAsync(PersonIdentifyReq personIdentify);
        Task<int> CreateTargetAsync(PersonIdentifyReq personIdentify);
        Task<Target> GetTargetByIdAsync(int id, PersonIdentifyReq personIdentify);
        Task<bool> DelTargetByIdAsync(int id, PersonIdentifyReq personIdentify);
    }

}

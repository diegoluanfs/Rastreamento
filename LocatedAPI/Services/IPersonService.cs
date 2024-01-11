using LocatedAPI.Models;
using LocatedAPI.Models.DTO;

namespace LocatedAPI.Services
{
    public interface IPersonService
    {
        Task<List<PersonResp>> GetAllPersonsAsync(PersonIdentifyReq? personIdentify);
        Task<PersonResp> GetPersonByIdAsync(int id);
        Task<PersonResp> GetPersonByUsernameAsync(string username);
        Task<bool> NameAlreadyExistsAsync(string name);
        Task<bool> EmailAlreadyExistsAsync(string name);
        Task<bool> ValidUserAndPassword(string username, string password);
        Task<int> CreatePersonAsync(PersonSignUpReq person);
        Task<bool> UpdatePersonAsync(PerfilReq person);
        Task<bool> DeletePersonAsync(int id);
        Task<string> AuthenticateAsync(PersonSignInReq person);
    }

}

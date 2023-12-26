using LocatedAPI.Models;
using LocatedAPI.Models.DTO;

namespace LocatedAPI.Repositories
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetAllAsync();
        Task<Person> GetPersonByIdAsync(int id);
        Task<Person> GetPersonByUsernameAsync(string username);
        Task<bool> NameAlreadyExistsAsync(string name);
        Task<bool> EmailAlreadyExistsAsync(string name);
        Task<List<Person>> GetPersonsByStringAsync(string searchParam);
        Task<Person> GetPersonByStringAsync(string searchParam);
        Task<bool> ValidUserAndPassword(string username, string password);
        Task<int> CreateAsync(Person person);
        Task<bool> UpdateAsync(Person person);
        Task<bool> DeleteAsync(int id);
    }

}

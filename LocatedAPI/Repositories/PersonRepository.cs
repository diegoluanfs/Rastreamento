using LocatedAPI.Data;
using LocatedAPI.Models;
using LocatedAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatedAPI.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly Contexto contexto;

        public PersonRepository(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<List<Person>> GetAllAsync()
        {
            try
            {
                return await contexto.Persons.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar todas as pessoas", ex);
            }
        }

        public async Task<Person> GetPersonByIdAsync(int id)
        {
            try
            {
                return await contexto.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o ID {id}", ex);
            }
        }

        public async Task<Person> GetPersonByUsernameAsync(string username)
        {
            try
            {
                return await contexto.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.Username == username);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o Username {username}", ex);
            }
        }

        public async Task<bool> NameAlreadyExistsAsync(string name)
        {
            try
            {
                var person = await contexto.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Username == name);

                return person != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao verificar se a pessoa com o Nome {name} existe", ex);
            }
        }

        public async Task<bool> EmailAlreadyExistsAsync(string email)
        {
            try
            {
                var person = await contexto.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Email == email);

                return person != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao verificar se a pessoa com o Nome {email} existe", ex);
            }
        }

        public async Task<List<Person>> GetPersonsByStringAsync(string searchParam)
        {
            try
            {
                return await contexto.Persons
                    .AsNoTracking()
                    .Where(p => p.Username == searchParam)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar pessoas por string", ex);
            }
        }

        public async Task<Person> GetPersonByStringAsync(string searchParam)
        {
            try
            {
                return await contexto.Persons
                    .AsNoTracking()
                    .Where(p => p.Email == searchParam || p.Username == searchParam)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar pessoa por string", ex);
            }
        }

        public async Task<int> CreateAsync(Person person)
        {
            try
            {
                await contexto.Persons.AddAsync(person);
                await contexto.SaveChangesAsync();
                return person.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao criar a pessoa", ex);
            }
        }

        public async Task<bool> ValidUserAndPassword(string username, string password)
        {
            try
            {
                var user = await contexto.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Username == username && p.Password == password);

                return user != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível validar usuário e senha!", ex);
            }
        }

        public async Task<bool> UpdateAsync(Person person)
        {
            try
            {
                // Utilize Attach para anexar o objeto ao contexto, se necessário
                contexto.Persons.Attach(person);

                // Ou utilize Update para começar a rastrear o objeto
                contexto.Persons.Update(person);

                await contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao atualizar a pessoa", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var person = await contexto.Persons.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                    return false;

                contexto.Persons.Remove(person);
                await contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao excluir a pessoa", ex);
            }
        }
    }
}

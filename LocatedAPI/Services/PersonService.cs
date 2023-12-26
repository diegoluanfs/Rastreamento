﻿using LocatedAPI.Models;
using LocatedAPI.Models.DTO;
using LocatedAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatedAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personRepository;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public PersonService(IPersonRepository personRepository, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.personRepository = personRepository;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }

        public async Task<List<PersonResp>> GetAllPersonsAsync()
        {
            try
            {
                var persons = await personRepository.GetAllAsync();

                if (persons == null || !persons.Any())
                {
                    return new List<PersonResp>();
                }

                var personsResp = persons.Select(person => new PersonResp(person)).ToList();

                return personsResp;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar todas as pessoas", ex);
            }
        }

        public async Task<PersonResp> GetPersonByIdAsync(int id)
        {
            try
            {
                Person person = await personRepository.GetPersonByIdAsync(id);
                if (person == null)
                {
                    return null;
                }

                PersonResp personResp = new PersonResp();
                personResp.Id = id;
                personResp.Username = person.Username;
                personResp.Email = person.Email;

                return new PersonResp(person);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o ID {id}", ex);
            }
        }

        public async Task<PersonResp> GetPersonByUsernameAsync(string username)
        {
            try
            {
                Person person = await personRepository.GetPersonByUsernameAsync(username);
                if (person == null)
                {
                    return null;
                }

                PersonResp personResp = new PersonResp();
                personResp.Id = person.Id;
                personResp.Username = person.Username;
                personResp.Email = person.Email;

                return new PersonResp(person);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o nome {username}", ex);
            }
        }

        public async Task<bool> NameAlreadyExistsAsync(string name)
        {
            try
            {
                return await personRepository.NameAlreadyExistsAsync(name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o ID {name}", ex);
            }
        }

        public async Task<bool> EmailAlreadyExistsAsync(string name)
        {
            try
            {
                return await personRepository.EmailAlreadyExistsAsync(name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o ID {name}", ex);
            }
        }

        public async Task<int> CreatePersonAsync(PersonSignUpReq personSignUpReq)
        {
            try
            {
                //if (await NameAlreadyExistsAsync(personSignUpReq.Username))
                //{
                //    throw new Exception("Já existe um usuário com o nome " + personSignUpReq.Username);
                //}

                //if (await EmailAlreadyExistsAsync(personSignUpReq.Email))
                //{
                //    throw new Exception("Já existe um usuário cadastrado com esse e-mail " + personSignUpReq.Email);
                //}

                Person person = new Person();
                person.Username = personSignUpReq.Username;
                person.Password = personSignUpReq.Password;
                person.Email = personSignUpReq.Email;

                return await personRepository.CreateAsync(person);
            }
            catch (Exception ex)
            {
                // Inclua detalhes específicos da exceção relevante para a resposta.
                throw new Exception("Houve um erro ao criar a pessoa.", ex);
            }
        }

        public async Task<bool> UpdatePersonAsync(PersonReq personReq)
        {
            try
            {
                Person person = new Person();
                person.Id = personReq.Id;
                person.Username = personReq.Username;
                person.Email = personReq.Email;
                person.Password = personReq.Password;

                return await personRepository.UpdateAsync(person);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao atualizar a pessoa com o ID {personReq.Id}", ex);
            }
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            try
            {
                return await personRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao excluir a pessoa com o ID {id}", ex);
            }
        }

        public async Task<bool> ValidUserAndPassword(string username, string password)
        {
            try
            {
                return await personRepository.ValidUserAndPassword(username, password);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível validar usuário e senha!", ex);
            }
        }

        public async Task<string> AuthenticateAsync(PersonSignInReq personIn)
        {
            try
            {

                PersonResp person = await GetPersonByUsernameAsync(personIn.Username);

                if (await ValidUserAndPassword(personIn.Username, personIn.Password))
                {
                    return await jWTAuthenticationManager.AuthenticateAsync(personIn.Username, personIn.Username, person.Id);
                }
                else
                {
                    throw new Exception("Credenciais inválidas");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao autenticar", ex);
            }
        }
    }
}
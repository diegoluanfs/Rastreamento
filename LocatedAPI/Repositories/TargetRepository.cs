using LocatedAPI.Data;
using LocatedAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatedAPI.Repositories
{
    public class TargetRepository : ITargetRepository
    {
        private readonly Contexto contexto;

        public TargetRepository(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<int> CreateTargetAsync(Target target)
        {
            try
            {
                await contexto.Targets.AddAsync(target);
                await contexto.SaveChangesAsync();
                return target.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao criar a target", ex);
            }
        }

        public async Task<bool> DelTargetByIdAsync(int id, int idPerson)
        {
            try
            {
                var targetToDelete = await contexto.Targets
                    .FirstOrDefaultAsync(t => t.Id == id && t.IdPerson == idPerson);

                if (targetToDelete != null)
                {
                    contexto.Targets.Remove(targetToDelete);
                    await contexto.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao excluir o alvo", ex);
            }
        }

        public async Task<List<Target>> GetAllTargetsAsync(int personId)
        {
            try
            {
                return await contexto.Targets
                    .AsNoTracking()
                    .Where(t => t.IdPerson == personId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar todos os targets para o person com o ID {personId}", ex);
            }
        }

        public async Task<Target> GetTargetByIdAsync(int id, int personId)
        {
            try
            {
                return await contexto.Targets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id && p.IdPerson == personId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar o target com o ID {id} para o person com o ID {personId}", ex);
            }
        }
    }
}

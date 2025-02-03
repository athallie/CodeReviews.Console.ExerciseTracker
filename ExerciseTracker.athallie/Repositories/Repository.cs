
using ExerciseTracker.athallie.Model;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.athallie.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly Context<TEntity> Context;
        public Repository(Context<TEntity> context)
        {
            Context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                await Context.AddAsync(entity);
                await Context.SaveChangesAsync();
                return entity;
            } catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public Task DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return Context.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} can't be null");
            }

            try
            {
                Context.Update(entity);
                await Context.SaveChangesAsync();
                return entity;
            } catch(Exception ex)
            {
                throw new Exception($"{nameof(entity)} can't be updated: {ex.Message}");
            }
        }
    }
}

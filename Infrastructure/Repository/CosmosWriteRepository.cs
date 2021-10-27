using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CosmosWriteRepository<TEntity> : ICosmosWriteRepository<TEntity> where TEntity : class, new()
    {
        private readonly APIContext _apiContext;

        public CosmosWriteRepository(APIContext apiContext)
        {
            _apiContext = apiContext;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _apiContext.AddAsync(entity);
            await _apiContext.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(TEntity entity)
        {
            _apiContext.Remove(entity);
            await _apiContext.SaveChangesAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _apiContext.Update(entity);
            await _apiContext.SaveChangesAsync();

            return entity;
        }
    }
}

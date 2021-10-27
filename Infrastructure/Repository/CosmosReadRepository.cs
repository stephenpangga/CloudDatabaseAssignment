using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CosmosReadRepository<TEntity> : ICosmosReadRepository<TEntity> where TEntity : class, new()
    {
        protected readonly APIContext _APIContext;

        public CosmosReadRepository(APIContext apiContext)
        {
            _APIContext = apiContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            var results = _APIContext.Set<TEntity>();

            if(results == null)
            {
                throw new Exception("error occured: couldn't retrieve data");

            }
            return results;
        }
    }
}

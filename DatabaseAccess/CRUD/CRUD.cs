using Contracts.SQLDB;
using DatabaseAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.CRUD
{
    public class CRUD<T> : ISQLCRUD<T> where T : class, BaseEntity
    {
        public async Task AddOrUpdateMultipleEntities(IEnumerable<T> entities)
        {
            using var dbCtx = DBContextFactory.Instance.GetDBContext();

            foreach (var entity in entities)
            {
                if (dbCtx.Set<T>().Any(e => e.Id == entity.Id))
                {
                    dbCtx.Set<T>().Update(entity);
                }
                else
                {
                    dbCtx.Set<T>().Add(entity);
                }
            }

            await dbCtx.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllEntities()
        {
            using var dbCtx = DBContextFactory.Instance.GetDBContext();
            return dbCtx.Set<T>().ToList();
        }
    }
}

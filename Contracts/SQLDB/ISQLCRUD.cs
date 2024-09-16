using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SQLDB
{
    public interface ISQLCRUD<T> where T : class, BaseEntity
    {
        Task AddOrUpdateMultipleEntities(IEnumerable<T> entities);
        IEnumerable<T> GetAllEntities();
    }
}

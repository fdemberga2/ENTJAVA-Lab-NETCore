using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IRelicRepository
    {
        RelicEntity GetSingle(int id);
        void Add(RelicEntity item);
        void Delete(int id);
        RelicEntity Update(int id, RelicEntity item);
        IQueryable<RelicEntity> GetAll(QueryParameters queryParameters);
        ICollection<RelicEntity> GetRandomRelic();
        int Count();
        bool Save();
    }
}

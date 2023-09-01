using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class RelicSqlRepository : IRelicRepository
    {
        private readonly RelicDbContext _relicDbContext;

        public RelicSqlRepository(RelicDbContext relicDbContext)
        {
            _relicDbContext = relicDbContext;
        }

        public RelicEntity GetSingle(int id)
        {
            return _relicDbContext.RelicItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(RelicEntity item)
        {
            _relicDbContext.RelicItems.Add(item);
        }

        public void Delete(int id)
        {
            RelicEntity relicItem = GetSingle(id);
            _relicDbContext.RelicItems.Remove(relicItem);
        }

        public RelicEntity Update(int id, RelicEntity item)
        {
            _relicDbContext.RelicItems.Update(item);
            return item;
        }

        public IQueryable<RelicEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<RelicEntity> _allItems = _relicDbContext.RelicItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Attack.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _relicDbContext.RelicItems.Count();
        }

        public bool Save()
        {
            return (_relicDbContext.SaveChanges() >= 0);
        }

        public ICollection<RelicEntity> GetRandomRelic()
        {
            List<RelicEntity> toReturn = new List<RelicEntity>();

            toReturn.Add(GetRandomRelic("Helmet"));
            toReturn.Add(GetRandomRelic("Cape"));
            toReturn.Add(GetRandomRelic("Boots"));

            return toReturn;
        }

        private RelicEntity GetRandomRelic(string type)
        {
            return _relicDbContext.RelicItems
                .Where(x => x.Type == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}

using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(RelicDbContext relicContext)
        {
            relicContext.RelicItems.Add(new RelicEntity() { Attack = 1000, Type = "Helmet", Name = "Helmet of Barbs", Created = DateTime.Now });
            relicContext.RelicItems.Add(new RelicEntity() { Attack = 1100, Type = "Cape", Name = "Not-a-cape", Created = DateTime.Now });
            relicContext.RelicItems.Add(new RelicEntity() { Attack = 1200, Type = "Boots", Name = "Theif's escape", Created = DateTime.Now });
            relicContext.RelicItems.Add(new RelicEntity() { Attack = 1300, Type = "Cape", Name = "Cloak of Kafka", Created = DateTime.Now });

            relicContext.SaveChanges();
        }
    }
}

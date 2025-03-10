﻿using SampleWebApiAspNetCore.Repositories;
using SampleWebApiAspNetCore.Services;

namespace SampleWebApiAspNetCore.Helpers
{
    public static class SeedDataExtension
    {
        public static void SeedData(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var relicDbContext = scope.ServiceProvider.GetRequiredService<RelicDbContext>();
                var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();

                seedDataService.Initialize(relicDbContext);
            }
        }
    }
}

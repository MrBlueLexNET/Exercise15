using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        private static LmsApiContext db;

        public static async Task InitAsync(LmsApiContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            db = context;


            if (await db.Tournament.AnyAsync()) return;

            var faker = new Faker("fr");
            var tournaments = new List<Tournament>();

            for (int i = 0; i < 20; i++)
            {
                tournaments.Add(new Tournament
                {
                    Title = faker.Company.CompanySuffix() + faker.Random.Word(),
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(-20, 20)),
                    Games = new Game[]
                    {
                            new Game
                            {
                              Name = faker.Commerce.ProductName(),
                             Description = faker.Commerce.ProductDescription(),
                             Time = DateTime.Now.AddHours(faker.Random.Int(-20, 20))
                          }
                    }
                });

            }

            db.AddRange(tournaments);
            await db.SaveChangesAsync();

        }

    }
}


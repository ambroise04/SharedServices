using SharedServices.DAL.Entities;
using System.Linq;
using System.Collections.Generic;

namespace SharedServices.DAL.Seeds
{
    public static class ServiceGategorySeed
    {
        public static ServiceGroup GetById(int id)
        {
            return GetCategories().First(c => c.Id == id);
        }
        public static List<ServiceGroup> GetCategories()
        {
            return new List<ServiceGroup>
            {
                new ServiceGroup
                {
                    Id = 1,
                    Title = "Déménagement",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 2,
                    Title = "Jardinage",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 3,
                    Title = "Bricolage",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 4,
                    Title = "Babysitting",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 5,
                    Title = "Animaux",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 6,
                    Title = "Cours particuliers",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 7,
                    Title = "Tâches ménagères",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 8,
                    Title = "Informatique",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 9,
                    Title = "Soins et Beauté",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 10,
                    Title = "Evénements",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 11,
                    Title = "Entretien",
                    PointsByHour = 15,
                },
                new ServiceGroup
                {
                    Id = 12,
                    Title = "Courses et Démarches",
                    PointsByHour = 15,
                },
            };
        }
    }
}
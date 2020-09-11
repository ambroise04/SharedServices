using SharedServices.DAL.Entities;
using System.Collections.Generic;

namespace SharedServices.DAL.Seeds
{
    public static class SeedServices
    {
        public static List<Service> GetServices()
        {
            return new List<Service>
            {
                new Service 
                {
                    Id = 1,
                    Title = "Garde d'enfant",
                    Description = "Garder les enfants, les amuser le temps que les parents reviennent.",
                    GroupId = 4
                },
                new Service 
                {
                    Id = 8,
                    Title = "Garde de personnes agées",
                    Description = "Garder les grands-parents, les aider et leur faire rire.",
                    GroupId = 4
                },
                new Service 
                {
                    Id = 2,
                    Title = "Aide déménagement",
                    Description = "Vous aider à déplacer vos meubles et autres.",
                    GroupId = 1
                },
                new Service 
                {
                    Id = 3,
                    Title = "Montage de meubles en kit",
                    Description = "Habitué des meubles, je peux vous aider à monter les vôtres.",
                    GroupId = 3
                },
                new Service 
                {
                    Id = 4,
                    Title = "Garde d'animaux",
                    Description = "Fan des animaux, j'adore les garder et les promener",
                    GroupId = 5
                },
                new Service 
                {
                    Id = 5,
                    Title = "Maçonnerie",
                    Description = "Je peux vous aider à faire les finitions",
                    GroupId = 3
                },
                new Service 
                {
                    Id = 6,
                    Title = "Décoration",
                    Description = "Aide à la décoration pour vos événements.",
                    GroupId = 3
                },
                new Service 
                {
                    Id = 7,
                    Title = "Tondre une pelouse",
                    Description = "Garder les enfants, les amuser le temps que les parents reviennent",
                    GroupId = 2
                },
                new Service 
                {
                    Id = 9,
                    Title = "Tondre une haie",
                    Description = "Rendre la clôture de vos maisons belle et attirante.",
                    GroupId = 2
                }
                ,
                new Service 
                {
                    Id = 10,
                    Title = "Aide ménagère",
                    Description = "Vous aider dans vos différentes tâches ménagères.",
                    GroupId = 7
                },
                new Service 
                {
                    Id = 11,
                    Title = "Repassage",
                    Description = "Repasser vos linges.",
                    GroupId = 7
                },
            };
        }
    }
}
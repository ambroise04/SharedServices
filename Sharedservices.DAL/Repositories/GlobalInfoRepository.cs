using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Interfaces;
using SharedServices.Mutual;
using System;
using System.Linq;

namespace SharedServices.DAL.Repositories
{
    public class GlobalInfoRepository : IGlobalInfoRepository
    {
        private readonly ApplicationContext Context;
        public GlobalInfoRepository(ApplicationContext context)
        {
            Context = context;
        }

        public GlobalInfo GetInfo()
        {
            return Context.Infos.FirstOrDefault();
        }

        public GlobalInfo Update(GlobalInfo entity)
        {
            if (entity is null)
                throw new ArgumentException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id <= 0)
                throw new ArgumentException($"Invalid object was submitted. The object has a bad id. {nameof(entity)}");

            Context.Attach(entity).State = EntityState.Modified;

            return entity;
        }
    }
}
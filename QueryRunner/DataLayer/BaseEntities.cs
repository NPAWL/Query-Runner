using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer
{
    public static class BaseEntities
    {
        public static QueryRunnerEntities Context { get { return new QueryRunnerEntities(); } }

        public static void Delete<TEntity>(this QueryRunnerEntities context, TEntity entity) where TEntity : class
        {
            if (context.Entry(entity).State == System.Data.Entity.EntityState.Detached)
                context.Set<TEntity>().Attach(entity);
            context.Set<TEntity>().Remove(entity);
        }

        public static TEntity Insert<TEntity>(this QueryRunnerEntities context, TEntity entity) where TEntity : class
        {
            return context.Set<TEntity>().Add(entity);
        }

        public static void Update<TEntity>(this QueryRunnerEntities context, TEntity entity) where TEntity : class
        {
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
    }
}

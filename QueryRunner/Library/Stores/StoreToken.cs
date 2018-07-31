using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entities;
using Library.Interfaces;
using Library.Models;

namespace Library.Stores
{
    public class StoreToken : IToken
    {
        private QueryRunnerEntities _ctx;

        public StoreToken() { }
        public StoreToken(QueryRunnerEntities context)
        {
            _ctx = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<ModelToken> ReadTokens()
        {
            var model = new ModelToken();
            return model.Get(_ctx);
        }

        public ModelToken GetToken(int tokenid)
        {
            var model = new ModelToken();
            return model.Get(_ctx).FirstOrDefault(x => x.TokenID == tokenid);
        }

        public IQueryable<ModelToken> GetTokensByUsername(string username)
        {
            var model = new ModelToken();
            return model.Get(_ctx).Where(x => x.Username == username);
        }

        public IQueryable<ModelToken> GetTokensByTest(int testid)
        {
            var model = new ModelToken();
            return model.Get(_ctx).Where(x => x.TestID == testid);
        }

        public IQueryable<ModelToken> GetTokensByTokenString(string tokenstring)
        {
            var model = new ModelToken();
            return model.Get(_ctx).Where(x => x.TokenString == tokenstring);
        }

        public void CreateToken(ModelToken model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.TokenActive = true;     

                    var entity = model.ToEntity();

                    _ctx.Insert(entity);
                    _ctx.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void CreateToken(ModelToken model, string username, int testid)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.TokenActive = true;
                    model.Username = username;
                    model.TestID = testid;

                    var entity = model.ToEntity();

                    _ctx.Insert(entity);
                    _ctx.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void CreateToken(ModelToken model, string[] usernames, int testid)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.TokenActive = true;
                    model.TestID = testid;

                    Each(usernames, x => {

                    model.Username = x;

                    var entity = model.ToEntity();

                    _ctx.Insert(entity);
                    _ctx.SaveChanges();  
                    
                    });

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void DeleteToken(ModelToken model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Tokens.FirstOrDefault(x => x.TokenString == model.TokenString);

                    entity.TokenActive = false;

                    _ctx.Update(entity);
                    _ctx.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void UpdateToken(ModelToken model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Tokens.FirstOrDefault(x => x.TokenString == model.TokenString);

                    model.Update(entity);

                    _ctx.Update(entity);
                    _ctx.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void Each<T>(IEnumerable<T> items, Action<T> action)
        {
          foreach (var item in items)
            action(item);
        }

    }
}

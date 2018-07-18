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
    public class StoreTest : ITest
    {
        private QueryRunnerEntities _ctx;

        public StoreTest() { }
        public StoreTest(QueryRunnerEntities context)
        {
            _ctx = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<ModelTest> ReadTests()
        {
            var model = new ModelTest();
            return model.Get(_ctx);
        }

        public ModelTest GetTest(int testid)
        {
            var model = new ModelTest();
            return model.Get(_ctx).FirstOrDefault(x => x.TestID == testid);
        }

        public ModelTest GetTestByName(string testname)
        {
            var model = new ModelTest();
            return model.Get(_ctx).FirstOrDefault(x => x.TestName == testname);
        }

        public void CreateTest(ModelTest model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.TestActive = true;     

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

        public void CreateTest(ModelTest model, string username, List<ModelQuestion> questions)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.TestActive = true;   
                    model.Username = username;

                    var entity = model.ToEntity();

                    Test test = _ctx.Insert(entity);
                    _ctx.SaveChanges();

                    Each(questions, x => (new StoreQuestion(_ctx)).CreateQuestion(x, test.TestID));

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void DeleteTest(ModelTest model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Tests.FirstOrDefault(x => x.TestID == model.TestID);

                    entity.TestActive = false;

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

        public void UpdateTest(ModelTest model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Tests.FirstOrDefault(x => x.TestID == model.TestID);

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

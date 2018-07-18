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
    public class StoreQuestion : IQuestion
    {
        private QueryRunnerEntities _ctx;

        public StoreQuestion() { }
        public StoreQuestion(QueryRunnerEntities context)
        {
            _ctx = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<ModelQuestion> ReadQuestions()
        {
            var model = new ModelQuestion();
            return model.Get(_ctx);
        }

        public ModelQuestion GetQuestion(int qid)
        {
            var model = new ModelQuestion();
            return model.Get(_ctx).FirstOrDefault(x => x.QuestionID == qid);
        }

        public IQueryable<ModelQuestion> GetQuestionsByTest(int testid)
        {
            var model = new ModelQuestion();
            return model.Get(_ctx).Where(x => x.TestID == testid);
        }

        public void CreateQuestion(ModelQuestion model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.QuestionActive = true;     

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

        public void CreateQuestion(ModelQuestion model, int testid)
        {
            //using (var transaction = _ctx.Database.BeginTransaction())
            //{
                try
                {
                    model.QuestionActive = true; 
                    model.TestID = testid;

                    var entity = model.ToEntity();

                    _ctx.Insert(entity);
                    _ctx.SaveChanges();

                    //transaction.Commit();
                }
                catch (Exception e)
                {
                    //transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            //}
        }

        public void DeleteQuestion(ModelQuestion model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Questions.FirstOrDefault(x => x.QuestionID == model.QuestionID);

                    entity.QuestionActive = false;

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

        public void UpdateQuestion(ModelQuestion model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Questions.FirstOrDefault(x => x.QuestionID == model.QuestionID);

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

    }
}

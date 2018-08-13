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
    public class StoreStudentAnswer : IStudentAnswer
    {
        private QueryRunnerEntities _ctx;

        public StoreStudentAnswer() { }
        public StoreStudentAnswer(QueryRunnerEntities context)
        {
            _ctx = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<ModelStudentAnswer> ReadStudentAnswers()
        {
            var model = new ModelStudentAnswer();
            return model.Get(_ctx);
        }

        public ModelStudentAnswer GetStudentAnswer(int said)
        {
            var model = new ModelStudentAnswer();
            return model.Get(_ctx).FirstOrDefault(x => x.SAID == said);
        }

        public IQueryable<ModelStudentAnswer> GetStudentAnswersByStudent(string username)
        {
            var model = new ModelStudentAnswer();
            return model.Get(_ctx).Where(x => x.Username == username);
        }

        public IQueryable<ModelStudentAnswer> GetStudentAnswersByTest(int testid)
        {
            //TODO: needs testing
            var model = new ModelStudentAnswer();
            StoreQuestion questionStore = new StoreQuestion(_ctx);
            IQueryable<ModelQuestion> questions = questionStore.GetQuestionsByTest(testid);
            List<ModelStudentAnswer> final = new List<ModelStudentAnswer>();
            model.Get(_ctx).ToList().ForEach(cur =>
            {
                ModelQuestion qq = questionStore.GetQuestion(cur.QuestionID);
                foreach (ModelQuestion curQQ in questions)
                {
                    if (curQQ.QuestionID == cur.QuestionID)
                        final.Add(cur);
                }
            });
            return final.AsQueryable();
        }

        public IQueryable<ModelStudentAnswer> GetStudentAnswersByQuestion(int qid)
        {
            var model = new ModelStudentAnswer();
            return model.Get(_ctx).Where(x => x.QuestionID == qid);
        }

        public IQueryable<ModelStudentAnswer> GetStudentAnswersByStudentByTest(string username, int testid)
        {
            var model = new ModelStudentAnswer();
            StoreQuestion questionStore = new StoreQuestion(_ctx);
            List<ModelQuestion> questions = questionStore.GetQuestionsByTest(testid).ToList();
            List<ModelStudentAnswer> modelQuestion = model.Get(_ctx).Where(x => x.Username == username).ToList();
            List<ModelStudentAnswer> final = new List<ModelStudentAnswer>();
            modelQuestion.ForEach(cur =>
            {
                ModelQuestion qq = questionStore.GetQuestion(cur.QuestionID);
                foreach (ModelQuestion curQQ in questions)
                {
                    if (curQQ.QuestionID == cur.QuestionID)
                        final.Add(cur);
                }
            });
            return final.AsQueryable();
        }

        public void CreateStudentAnswer(ModelStudentAnswer model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.StudentAnswerActive = true;

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

        public void CreateStudentAnswer(ModelStudentAnswer model, string username, int qid)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.StudentAnswerActive = true;
                    model.Username = username;
                    model.QuestionID = qid;

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

        public void DeleteStudentAnswer(ModelStudentAnswer model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.StudentAnswers.FirstOrDefault(x => x.SAID == model.SAID);

                    entity.StudentAnswerActive = false;

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

        public void UpdateStudentAnswer(ModelStudentAnswer model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.StudentAnswers.FirstOrDefault(x => x.SAID == model.SAID);

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

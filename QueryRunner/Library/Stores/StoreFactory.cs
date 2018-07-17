using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Library.Interfaces;

namespace Library.Stores
{
    public static class StoreFactory
    {
        public static IRole GetRole(QueryRunnerEntities context)
        {
            return new StoreRole(context);
        }

        public static IUser GetUser(QueryRunnerEntities context)
        {
            return new StoreUser(context);
        }

        public static IUserRole GetUserRole(QueryRunnerEntities context)
        {
            return new StoreUserRole(context);
        }

        public static IUserLogin GetUserLogin(QueryRunnerEntities context)
        {
            return new StoreUserLogin(context);
        }

        public static IQuestion GetQuestion(QueryRunnerEntities context)
        {
            return new StoreQuestion(context);
        }

        public static ITest GetTest(QueryRunnerEntities context)
        {
            return new StoreTest(context);
        }

        public static IStudentAnswer GetStudentAnswer(QueryRunnerEntities context)
        {
            return new StoreStudentAnswer(context);
        }

        public static IToken GetToken(QueryRunnerEntities context)
        {
            return new StoreToken(context);
        }

    }
}

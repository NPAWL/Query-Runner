using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using DataLayer.Entities;
using Library.Interfaces;
using Library.Stores;

namespace QueryRunner.App_Start
{
    public class DependencyConfig
    {
        public static void RegisterAll()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<QueryRunnerEntities>().AsSelf();

            builder.Register(x => StoreFactory.GetRole(new QueryRunnerEntities())).As<IRole>();
            builder.Register(x => StoreFactory.GetUser(new QueryRunnerEntities())).As<IUser>();
            builder.Register(x => StoreFactory.GetUserRole(new QueryRunnerEntities())).As<IUserRole>();
            builder.Register(x => StoreFactory.GetTest(new QueryRunnerEntities())).As<ITest>();
            builder.Register(x => StoreFactory.GetStudentAnswer(new QueryRunnerEntities())).As<IStudentAnswer>();
            builder.Register(x => StoreFactory.GetUserLogin(new QueryRunnerEntities())).As<IUserLogin>();
            builder.Register(x => StoreFactory.GetQuestion(new QueryRunnerEntities())).As<IQuestion>();
            builder.Register(x => StoreFactory.GetToken(new QueryRunnerEntities())).As<IToken>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
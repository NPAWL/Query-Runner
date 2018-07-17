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
            builder.Register(x => StoreFactory.GetSkip(new QueryRunnerEntities())).As<ISkip>();
            builder.Register(x => StoreFactory.GetSkipTask(new QueryRunnerEntities())).As<ISkipTask>();
            builder.Register(x => StoreFactory.GetCustomer(new QueryRunnerEntities())).As<ICustomer>();
            builder.Register(x => StoreFactory.GetTask(new QueryRunnerEntities())).As<ITask>();
            builder.Register(x => StoreFactory.GetTask_toCust(new QueryRunnerEntities())).As<ITask_toCust>();
            builder.Register(x => StoreFactory.GetTask_toDump(new QueryRunnerEntities())).As<ITask_toDump>();
            builder.Register(x => StoreFactory.GetTask_toYard(new QueryRunnerEntities())).As<ITask_toYard>();
            builder.Register(x => StoreFactory.GetPhoto(new QueryRunnerEntities())).As<IPhoto>();
            builder.Register(x => StoreFactory.GetSkip_Task_Photo(new QueryRunnerEntities())).As<ISkip_Task_Photo>();
            builder.Register(x => StoreFactory.GetCheckIn(new QueryRunnerEntities())).As<ICheckIn>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
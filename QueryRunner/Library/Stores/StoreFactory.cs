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
            return new Store_Role(context);
        }

        public static IUser GetUser(QueryRunnerEntities context)
        {
            return new Store_User(context);
        }

        public static IUserRole GetUserRole(QueryRunnerEntities context)
        {
            return new Store_User_Role(context);
        }

        public static ICustomer GetCustomer(QueryRunnerEntities context)
        {
            return new Store_Customer(context);
        }

        public static ISkip GetSkip(QueryRunnerEntities context)
        {
            return new Store_Skip(context);
        }

        public static ITask GetTask(QueryRunnerEntities context)
        {
            return new Store_Task(context);
        }

        public static ISkipTask GetSkipTask(QueryRunnerEntities context)
        {
            return new Store_Skip_Task(context);
        }

        public static ITask_toCust GetTask_toCust(QueryRunnerEntities context)
        {
            return new Store_Task_toCust(context);
        }

        public static ITask_toDump GetTask_toDump(QueryRunnerEntities context)
        {
            return new Store_Task_toDump(context);
        }

        public static ITask_toYard GetTask_toYard(QueryRunnerEntities context)
        {
            return new Store_Task_toYard(context);
        }

        public static IPhoto GetPhoto(QueryRunnerEntities context)
        {
            return new Store_Photo(context);
        }

        public static ISkip_Task_Photo GetSkip_Task_Photo(QueryRunnerEntities context)
        {
            return new Store_Skip_Task_Photo(context);
        }

        public static ICheckIn GetCheckIn(QueryRunnerEntities context)
        {
            return new Store_CheckIn(context);
        }
    }
}

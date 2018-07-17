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
    }
}

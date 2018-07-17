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
    }
}

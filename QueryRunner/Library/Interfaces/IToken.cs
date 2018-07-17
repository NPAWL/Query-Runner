using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Interfaces
{
    public interface IToken
    {
        IQueryable<ModelToken> ReadTokens();
        ModelToken GetToken(string tokenstring);
        void CreateToken(ModelToken model);
        void DeleteToken(ModelToken model);
        void UpdateToken(ModelToken model);
    }
}

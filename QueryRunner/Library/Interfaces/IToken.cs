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
        IQueryable<ModelToken> GetTokensByUsername(string username);
        IQueryable<ModelToken> GetTokensByTest(int testid);
        void CreateToken(ModelToken model);
        void CreateToken(ModelToken model, string username, int testid);
        void CreateToken(ModelToken model, string[] usernames, int testid);
        void DeleteToken(ModelToken model);
        void UpdateToken(ModelToken model);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace Library.Models
{
    public class ModelToken
    {
        public string TokenString { get; set; }
        public string Username { get; set; }
        public int TestID { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ExpiredTime { get; set; }
        public bool TokenActive { get; set; }
        public int TokenID { get; set; }

        internal IQueryable<ModelToken> Get(QueryRunnerEntities context)
        {
            return from token in context.Tokens
                   select new ModelToken
                   {
                       TokenString = token.TokenString,
                       Username = token.Username,
                       TestID = token.TestID,
                       CreatedTime = token.CreatedTime,
                       ExpiredTime = token.ExpiredTime,
                       TokenActive = token.TokenActive,
                       TokenID = token.TokenID
                   };
        }

        public Token ToEntity()
        {
            return new Token
            {
                TokenString = TokenString,
                Username = Username,
                TestID = TestID,
                CreatedTime = CreatedTime,
                ExpiredTime = ExpiredTime,
                TokenActive = TokenActive//,
                //TokenID = TokenID
            };
        }

        public void Update(Token token)
        {
            token.TokenString = TokenString;
            token.Username = Username;
            token.TestID = TestID;
            token.CreatedTime = CreatedTime;
            token.ExpiredTime = ExpiredTime;
            token.TokenActive = TokenActive;
            //token.TokenID = TokenID;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleSQLCheck
{
    class Program
    {
        /**
         *  1.) Need to incorporate:
         *          SELECT P.ID FROM Person P 
         *              ==
         *          SELECT Person.ID FROM Person
         */

        public static void Main(string[] args)
        {
            //Splitting the statement into parts
            String question =
                "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age"; //Correct
            //String question = "SELECT Name, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age";//Select
            //String question = "SELECT Name, Surname, (Age+10) as Age From People P where ID = 5 order by Surname ASC Group by Age";//From
            //String question = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 AND Food = "YES!" order by Surname ASC Group by Age";//Where
            //String question = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname DESC Group by Age";//Order by
            //String question = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC";//group by

            //String question = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age";
            String answer =
                "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age";
            answer.Replace("\n", "");
            answer.Replace("\r", "");
            answer.Replace("\t", " ");
            answer.Replace("  ", " ");
            //NO SUBQUERIES! SELECT, FROM, WHERE, ORDER BY, GROUP BY
            string pattern_SFWOG = @"(.+?(?=from))|(.+(?= where))| (.+(?= order by))| (.+(?= group by))| (.*)";
            RegexOptions options = RegexOptions.IgnoreCase;
            List<String> parts_Q = Regex.Split(question, pattern_SFWOG, options).ToList();
            List<String> parts_A = Regex.Split(answer, pattern_SFWOG, options).ToList();
            parts_Q.RemoveAll(cur => cur.Equals(""));
            parts_A.RemoveAll(cur => cur.Equals(""));

            Console.WriteLine("Settings: RegexOptions.IgnoreCase\n");
            Console.WriteLine("Actual Answer: {0}\n", question);
            Console.WriteLine("Student Answer: {0}\n", answer);
            Console.WriteLine("Parts:");
            parts_A.ToList().ForEach(cur => Console.WriteLine("\t" + cur));

            double mark =
                doSelect(parts_A, parts_Q)
                + doFrom(parts_A, parts_Q)
                + doWhere(parts_A, parts_Q)
                + doGroupBy(parts_A, parts_Q)
                + doOrderBy(parts_A, parts_Q);
            mark /= 5;

            Console.WriteLine("Final mark is {:2}%", mark);

            Console.ReadLine();
        }

        private static double doSelect(List<String> answer, List<String> question)
        {
            //Checking SELECT
            String SELECT_Q = get("SELECT", question).Replace(" ", "");
            String SELECT_A = get("SELECT", answer).Replace(" ", "");
            List<String> attributes_Q = SELECT_Q.Split(',').ToList();
            List<String> attributes_A = SELECT_A.Split(',').ToList();
            double count = 0;
            attributes_Q.ForEach(cur =>
            {
                if (attributes_A.Contains(cur)) count++;
            });
            return count;
        }

        private static double doFrom(List<String> answer, List<String> question)
        {
            //Checking FROM
            String FROM_Q = get("FROM", question);
            String FROM_A = get("FROM", answer);
            List<String> attributes_Q = FROM_Q.Split(',').ToList();
            List<String> attributes_A = FROM_A.Split(',').ToList();
            double count = 0;
            attributes_Q.ForEach(cur =>
            {
                if (attributes_A.Contains(cur)) count++;
            });
            return count;
        }

        private static double doWhere(List<String> answer, List<String> question)
        {
            //Checking WHERE
            String WHERE_Q = get("WHERE", question);
            String WHERE_A = get("WHERE", answer);
            return 0;
        }

        private static double doGroupBy(List<String> answer, List<String> question)
        {
            //Checking ORDER BY
            String ODBY_Q = get("ORDER BY", question);
            String ODBY_A = get("ORDER BY", answer);
            return 0;
        }

        private static double doOrderBy(List<String> answer, List<String> question)
        {
            //Checking GROUP BY
            String GPBY_Q = get("GROUP BY", question);
            String GPBY_A = get("GROUP BY", answer);


            return 0;
        }

        #region [Helpers]

        private static String get(String selector, List<String> list)
        {
            foreach (String cur in list)
            {
                if (cur.ToUpper().Substring(0, selector.Length).Equals(selector))
                {
                    return cur.Substring(selector.Length, cur.Length - selector.Length);
                }
            }

            throw new BadSqlOperation("Selector not found in list!");
        }

        #endregion
    }

    class BadSqlOperation : Exception
    {
        public BadSqlOperation(string message) : base(message)
        {
            Console.WriteLine(message);
            Console.WriteLine(StackTrace);
        }
    }
}
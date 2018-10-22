using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleSQLCheck {
    public class SQL_NPAWL {
        public static void Main(string[] args) { }

        /**
         *  1.) Need to incorporate:
         *          SELECT P.ID FROM Person P 
         *              ==
         *          SELECT Person.ID FROM Person
         */
        const RegexOptions options = RegexOptions.IgnoreCase;

        public static int CalculateMarkForQuestion(string question, string answer, double weight) {
            //Splitting the statement into parts
            //string question =
            //    "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age"; //Correct
            //String question = "SELECT Name, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age";//Select
            //String question = "SELECT Name, Surname, (Age+10) as Age From People P where ID = 5 order by Surname ASC Group by Age";//From
            //String question = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 AND Food = "YES!" order by Surname ASC Group by Age";//Where
            //String question = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname DESC Group by Age";//Order by
            //String question = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC";//group by

            //String question = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age";
            //string answer =
            //"SELECT Name, Surname, (Age+10) as Age From People where ID=     5 order by Surname ASC Group by Age";
            answer = Regex.Replace(answer, "=", " = ");
            answer.Replace("\n", "");
            answer.Replace("\r", "");
            answer.Replace("\t", " ");
            answer.Replace("  ", " ");
            //NO SUBQUERIES! SELECT, FROM, WHERE, ORDER BY, GROUP BY
            const string pattern_SFWOG = @"(.+?(?=from))|(.+(?= where))| | (.+(?= group by))| (.*)";
            List<string> parts_Q = Regex.Split(question, pattern_SFWOG, options).ToList();
            List<string> parts_A = Regex.Split(answer, pattern_SFWOG, options).ToList();
            parts_Q.RemoveAll(cur => cur.Equals(""));
            parts_A.RemoveAll(cur => cur.Equals(""));

            Console.WriteLine("Settings: RegexOptions.IgnoreCase\n");
            Console.WriteLine("Actual Answer: {0}\n", question);
            Console.WriteLine("Student Answer: {0}\n", answer);
            Console.WriteLine("Parts:");
            parts_A.ToList().ForEach(cur => Console.WriteLine("\t" + cur));

            double mark =
                DoSelect(parts_A, parts_Q)
                + DoFrom(parts_A, parts_Q)
                + DoWhere(parts_A, parts_Q)
                + DoGroupBy(parts_A, parts_Q)
                + DoOrderBy(parts_A, parts_Q);
            mark /= 5;

            Console.WriteLine("Final mark is {0:2}%", mark * 100);
            return (int) Math.Round(mark * weight);
            // Console.ReadLine();
        }

        private static double DoSelect(IEnumerable<string> answer, IEnumerable<string> question) {
            //Checking SELECT
            try {
                string selectQ = Get("SELECT", question).Replace(" ", "");
                string selectA = Get("SELECT", answer).Replace(" ", "");
                if (selectQ.Equals("")) return 0;
                List<string> attributesQ = selectQ.Split(',').ToList();
                List<string> attributesA = selectA.Split(',').ToList();
                double count = 0.00;
                attributesQ.ForEach(cur => {
                    if (attributesA.Contains(cur)) count++;
                });
                return (count * 1.00) / attributesA.Count();
            }
            catch (BadSqlOperation e) {
                return 0;
            }
        }

        private static double DoFrom(IEnumerable<string> answer, IEnumerable<string> question) {
            //Checking FROM
            try {
                string fromQ = Get("FROM", question);
                string fromA = Get("FROM", answer);
                if (fromQ.Equals("")) return 0;
                List<string> attributesQ = fromQ.Split(',').ToList();
                List<string> attributesA = fromA.Split(',').ToList();
                double count = 0;
                attributesQ.ForEach(cur => {
                    if (attributesA.Contains(cur)) count++;
                });
                return (count * 1.00) / attributesA.Count();
            }
            catch (BadSqlOperation e) {
                return 0;
            }
        }

        private static double DoWhere(IEnumerable<string> answer, IEnumerable<string> question) {
            //Checking WHERE
            try {
                string whereQ = Get("WHERE", question);
                string whereA = Get("WHERE", answer);
                if (whereQ.Equals("")) return 0;
                List<String> conditionsQ = Regex.Split(whereQ, " and | or ", options).ToList();
                List<String> conditionsA = Regex.Split(whereA, " and | or ", options).ToList();
                double count = 0;
                conditionsQ.ForEach(cur => {
                    if (conditionsQ.Contains(cur)) count++;
                });
                return (count * 1.00) / conditionsA.Count();
            }
            catch (BadSqlOperation e) {
                return 0;
            }
        }

        private static double DoGroupBy(IEnumerable<string> answer, IEnumerable<string> question) {
            //Checking ORDER BY
            try {
                string odbyQ = Get("ORDER BY", question);
                string odbyA = Get("ORDER BY", answer);
                if (odbyQ.Equals("")) return 0;
                List<String> groupsQ = odbyQ.Split(',').ToList();
                List<String> groupsA = odbyA.Split(',').ToList();
                double count = 0;
                groupsQ.ForEach(cur => {
                    if (groupsQ.Contains(cur)) count++;
                });
                return (count * 1.00) / groupsA.Count();
            }
            catch (BadSqlOperation e) {
                return 0;
            }
        }

        private static double DoOrderBy(IEnumerable<string> answer, IEnumerable<string> question) {
            //Checking GROUP BY
            try {
                string GPBY_Q = Get("GROUP BY", question);
                string GPBY_A = Get("GROUP BY", answer);
                if (GPBY_Q.Equals("")) throw new BadSqlOperation("Selector not in use!");
                List<String> groupsQ = GPBY_Q.Split(',').ToList();
                List<String> groupsA = GPBY_A.Split(',').ToList();
                double count = 0;
                groupsQ.ForEach(cur => {
                    if (groupsQ.Contains(cur)) count++;
                });
                return (count * 1.00) / groupsA.Count();
            }
            catch (BadSqlOperation e) {
                return 0;
            }
        }


        #region [Helpers]

        private static string Get(string selector, IEnumerable<string> list) {
            try {
                foreach (string cur in list) {
                    String temp = cur.ToUpper().Substring(0, selector.Length);
                    if (cur.ToUpper().Substring(0, selector.Length).Equals(selector)) {
                        return cur.Substring(selector.Length, cur.Length - selector.Length);
                    }
                }
            }
            catch (ArgumentOutOfRangeException ignore) {
                throw new BadSqlOperation("Selector not found in list!");
            }

            throw new BadSqlOperation("Selector not found in list!");
        }

        #endregion
    }

    internal sealed class BadSqlOperation : Exception {
        public BadSqlOperation(string message) : base(message) {
            Console.WriteLine(message);
            Console.WriteLine(StackTrace);
        }
    }
}
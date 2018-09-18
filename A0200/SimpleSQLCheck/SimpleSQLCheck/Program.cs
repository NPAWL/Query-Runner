using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleSQLCheck {
    class Program {
        private static string pattern = @"(.+?(?=from))|(.+(?= where))| (.+(?= order by))| (.+(?= group by))| (.*)";

        public static void Main(string[] args) {
            //args[0] = "2";//Total marks
            //args[1] = "SELECT * FROM TableStudent";//Right Answer
            //args[2] = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age";//Student's Answer

            String question = "SELECT * FROM TableStudent";
            String answer = "SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age";

           // string input = @"SELECT Name, Surname, (Age+10) as Age From People where ID = 5 order by Surname ASC Group by Age";
            RegexOptions options = RegexOptions.IgnoreCase;

            List<String> parts = Regex.Split(answer, pattern, options).ToList();
            parts.RemoveAll(cur => cur.Equals(""));

            parts.ToList().ForEach(cur => Console.WriteLine(cur));
            Console.ReadLine();
        }

    }
}

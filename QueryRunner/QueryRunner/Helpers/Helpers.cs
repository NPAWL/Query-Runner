using DataLayer.Entities;
using Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace QueryRunner.Helpers
{
    public class Helper
    {
        public static void ExportToTextFile(HttpResponseBase response, StudentAnswer[] lines, double mark)
        {
            response.Clear();
            response.AddHeader("content-disposition", "attachment; filename=testfile.txt");
            response.AddHeader("content-type", "text/plain");
            //Textfile contents:
            using (StreamWriter writer = new StreamWriter(response.OutputStream))
            {
                foreach (StudentAnswer curAnswer in lines)
                {
                    //writer.WriteLine("{0}: {1}",curAnswer.QuestionID, curAnswer.Answer, curAnswer.);
                    
                }
            }
            response.End();
        }
   

        public static void FEach<T>(IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
                action(item);
        }



    }
}
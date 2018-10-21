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
    public class HelpingClass
    {
        public static MemoryStream ExportToTextFile(List<String> lines)
        {
            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);

            foreach (String line in lines)
            {
                tw.Write(line);
            }

            tw.Flush();
          
            return memoryStream;
        }


        public static void FEach<T>(IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
                action(item);
        }



    }
}
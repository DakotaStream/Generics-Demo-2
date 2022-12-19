using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
/**  Visual Studio Notes
 * CTRL . or Alt Enter Segestions
 *    Move a class into its own file.
 *    Remove Usings that are not referenced 
 * F12 If on a Class.Something it will take you there.
 *   CTRL click also.
 * CTRL F12 Takes you to the implementation of the interface.
 * CTRL T or CTRL comma: better search
 * CTRL K CTRL D Format doc
 * 
 * Options > Text Editor > Code Cleanup
 * 
 * Code View> Other Windows> C# Interactive:snippit playground
 * 
 * Alt Down Arrow: Move a line of code.
 * 
 * CTRL Shift Space: Parameters
 * 
 * CTRL J Pick a different method from class.
 *
 * 
 * Shift Alt: up and down, click a drag. Change multiple line
 */

namespace ConsoleUI
{
    internal class GenericTextFileProcessor//  <S>  You can do it at class level.
    {
        // Limitations/ constraint
        // Must be class
        // 2 Must have an empty constructor new().  Not required, but helps you dial in what type of object is put in T0
        //public static List<T, U> LoadFromTextFile<T> (string filePath) where T : class, new() where U : class
        public static List<T> LoadFromTextFile<T>(string filePath) where T : class, new()
        {
            var lines = System.IO.File.ReadAllLines(filePath).ToList();
            List<T> output = new List<T>();
            T entry = new T();  //Limit new() is here.  If limit is not used, this line will error out
            var cols = entry.GetType().GetProperties();  //Reflection used here.  Expensive.  Doing at file write so its ok.
            //Checks to be sure we have at least one header row and one data row
            if (lines.Count < 2)
            {
                throw new IndexOutOfRangeException("The file was either empty or missing.");
            }

            // Splits the header into one column header per entry
            var headers = lines[0].Split(',');

            // Removes the header row from the lines so we don't
            // have to worry about skipping over that first row.
            lines.RemoveAt(0);

            foreach (var row in lines)
            {
                entry = new T();

                // Splits the row into individual columns. Now the index
                // of this row matches the index of the header so the
                // FirstName column header lines up with the FirstName
                // value in this row.
                var vals = row.Split(',');

                // Loops through each header entry so we can compare that
                // against the list of columns from reflection. Once we get
                // the matching column, we can do the "SetValue" method to
                // set the column value for our entry variable to the vals
                // item at the same index as this particular header.
                for (var i = 0; i < headers.Length; i++)
                {
                    foreach (var col in cols)
                    {

                        if (col.Name == headers[i])
                        {
                            col.SetValue(entry, Convert.ChangeType(vals[i], col.PropertyType));
                        }
                    }
                }
                output.Add(entry);
            }
            return output;
        }

        public static void SaveToTextFile<T>(List<T> data, string filePath) where T : class//, new() I guess we did not need this limit
        {
            List<string> lines = new List<string>();
            StringBuilder line = new StringBuilder();

            if (data == null || data.Count == 0)
            {
                throw new ArgumentNullException("data", "You must populate the data parameter with at least o");

            }
            var cols = data[0].GetType().GetProperties();
            // Loops through each column and gets the name so it can comma
            // separate it into the header row.
            foreach (var col in cols)
            {
                line.Append(col.Name);
                line.Append(",");
            }



            // Adds the column header entries to the first line (removing
            // the last comma from the end first).
            lines.Add(line.ToString().Substring(0, line.Length - 1));

            foreach (var row in data)
            {
                line = new StringBuilder();

                foreach (var col in cols)
                {
                    line.Append(col.GetValue(row));
                    line.Append(",");
                }

                // Adds the row to the set of lines (removing
                // the last comma from the end first).
                lines.Add(line.ToString().Substring(0, line.Length - 1));
            }

            System.IO.File.WriteAllLines(filePath, lines);
        }
    }
}



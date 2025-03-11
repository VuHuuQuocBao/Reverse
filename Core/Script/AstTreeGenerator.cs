using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Script
{
    public class AstTreeGenerator
    {
        public void DefineAst(string outputDir, string baseName, List<string> types)
        {
            string path = Path.Combine(outputDir, baseName + ".cs");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("using Compiler.Core;");
                writer.WriteLine("namespace Compiler.Core");
                writer.WriteLine("{");
                writer.WriteLine("abstract class " + baseName);
                writer.WriteLine("{");
                writer.WriteLine("}");
                writer.WriteLine("}");
                foreach (var type in types)
                {
                    string className = type.Split(':')[0].Trim();
                    string fields = type.Split(':')[1].Trim();
                    DefineTypeMethod(writer, baseName, className, fields);
                }
            }
        }

        public static void DefineTypeMethod(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine("    class " + className + " : " + baseName);
            writer.WriteLine("    {");
            // Constructor
            writer.WriteLine("        public " + className + "(" + fieldList + ")");
            writer.WriteLine("        {");

            // Store parameters in fields
            string[] fields = fieldList.Split(new[] { ", " }, StringSplitOptions.None);
            foreach (string field in fields)
            {
                string name = field.Split(' ')[1];
                writer.WriteLine("            this." + name + " = " + name + ";");
            }
            writer.WriteLine("        }");

            // Fields
            writer.WriteLine();
            foreach (string field in fields)
            {
                writer.WriteLine("        public readonly " + field + ";");
            }

            writer.WriteLine("    }");
        }
    }
}

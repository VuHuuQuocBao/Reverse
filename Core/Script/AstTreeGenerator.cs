﻿using System.Text;

namespace Core.Script
{
    public class AstTreeGenerator
    {
        public void DefineAst(string outputDir, string baseName, List<string> types)
        {
            string path = Path.Combine(outputDir, baseName + ".cs");

            // get all class from 
            (var listClassName, var lines) = GetAllClassNameFromFile(path);
            
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                if(lines is { Count : > 0})
                {
                    lines.RemoveAt(lines.Count - 1);
                    foreach(var line in lines)
                        writer.WriteLine(line);
                }
                else
                {
                    writer.WriteLine("namespace Core.Core");
                    writer.WriteLine("{");
                    writer.WriteLine("abstract class " + baseName);
                    writer.WriteLine("{");
                    writer.WriteLine("}");
                    writer.WriteLine("using Core.Core;");
                }

                foreach (var type in types)
                {
                    string className = type.Split(':')[0].Trim();

                    if (listClassName.Contains(className))
                        continue;

                    string fields = type.Split(':')[1].Trim();
                    DefineTypeMethod(writer, baseName, className, fields);
                }
                writer.Write("}");
            }
        }

        public static void DefineTypeMethod(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine("   public class " + className + " : " + baseName);
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

            (baseName switch
            {
                "Expression" => (Action)(() => writer.WriteLine("public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();")),
                "Statement" => (Action)(() => writer.WriteLine("public override T Accept<T>(IStatementVisitor<T> visitor) => throw new NotImplementedException();"))
            })();

            writer.WriteLine("    }");
        }

        public (List<string>, List<string>) GetAllClassNameFromFile(string path)
        {
            List<string> listClassName = new();
            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                // Trim leading and trailing whitespace
                string trimmedLine = line.Trim();

                // Check if the line contains "class" and ":"
                if (trimmedLine.Contains("class ") && trimmedLine.Contains(":") && !trimmedLine.StartsWith("//"))
                {
                    // Extract the class name by splitting the line
                    var parts = trimmedLine.Split(' ');

                    // The second word after "class" is typically the class name
                    if (parts.Length > 1)
                    {
                        var classIndex = Array.IndexOf(parts, "class");
                        listClassName.Add(parts[classIndex + 1]);
                    }
                }
            }

            return (listClassName, lines.ToList());
        }
    }
}

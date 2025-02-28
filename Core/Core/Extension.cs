using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Core
{
    public static class Extension
    {
        public static string Scan(string path)
        {
			try
			{
                var bytes = File.ReadAllBytes(path);

                var content = Encoding.Default.GetString(bytes);

                return content;
            }
            catch (Exception ex)
			{
                Console.WriteLine($"An error occurred: \n {ex.Message}");
                return null;
            }
        }

        //public static void Run()
    }
}

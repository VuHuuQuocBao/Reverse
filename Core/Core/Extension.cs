using System.Text;

namespace Core.Core
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

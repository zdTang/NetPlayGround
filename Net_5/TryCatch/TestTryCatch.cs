using System;
using System.IO;

namespace Net_5
{
    public class TestTryCatch
    {
        public static void Test()
        {
            // finally blocks are typically used for cleanup code:

            File.WriteAllText ("file.txt", "test");
            ReadFile ();

            ProperCase(null);     // throws an ArgumentException

        }

        
        static string ProperCase(string value) =>
            value == null ? throw new ArgumentException("value") :
            value == "" ? "MIKE" :
            char.ToUpper(value[0]) + value.Substring(1);

        static void ReadFile()
        {
            StreamReader reader = null;    // In System.IO namespace
            try
            {
                reader = File.OpenText ("file.txt");
                if (reader.EndOfStream) return;
                Console.WriteLine (reader.ReadToEnd());
            }
            finally
            {
                if (reader != null) reader.Dispose();
            }
        }
    }
}
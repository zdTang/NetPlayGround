using System.IO;

namespace IO
{
    public class BasicClass
    {
        public static void Test()
        {
            // Instantiate a Stream
            Stream s = new FileStream("test.txt", FileMode.Create);
            string sourceDirPath = @"demo.txt";
            DirectoryInfo directory = new DirectoryInfo(sourceDirPath);
            bool directoryExists = directory.Exists;



        }
    }
}
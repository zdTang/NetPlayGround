using System;
using System.IO;

namespace IO
{

    
    
    class StreamReaderWriter
    {  
        static void Test(string[] args)  
        {  
            Console.ForegroundColor = ConsoleColor.Green;  
            Console.Title = "StreamReader and STreamWriter";  
            WriteToFile();  
            ReadFromFile();  
            Console.ForegroundColor = ConsoleColor.Gray;  
        }  
  
        public static void ReadFromFile()  
        {  
            using (StreamReader sr = File.OpenText(@"E:\Programming Practice\CSharp\Console\table.tbl"))  
            {  
                string tables = null;  
                  
                while ((tables = sr.ReadLine()) != null)  
                {  
                    Console.WriteLine("{0}",tables);  
                }  
                Console.WriteLine("Table Printed.");  
            }  
        }  
  
        public static void WriteToFile()  
        {  
            using (StreamWriter sw = File.CreateText(@"E:\Programming Practice\CSharp\Console\table.tbl"))  
            {  
                sw.WriteLine("Please find the below generated table of 1 to 10");  
                sw.WriteLine("");  
                for (int i = 1; i <= 10; i++)  
                {  
                    for (int j = 1; j <= 10; j++)  
                    {  
                        sw.WriteLine("{0}x{1}= {2}",i,j,(i*j));  
                    }  
                    sw.WriteLine("==============");  
                }  
                Console.WriteLine("Table successfully written on file.");  
            }  
        }

        // create StreamReader directly
        public static void TestTwo()
        {
            // File name  
            string fileName = @"C:\Temp\CSharpAuthors.txt";  
            try  
            {  
                // Create a StreamReader  
                using (StreamReader reader = new StreamReader(fileName))  
                {  
                    string line;  
                    // Read line by line  
                    while ((line = reader.ReadLine()) != null)  
                    {  
                        Console.WriteLine(line);  
                    }  
                }  
            }  
            catch (Exception exp)  
            {  
                Console.WriteLine(exp.Message);  
            }  
            Console.ReadKey(); 
        }

        // create StreamWriter directly
        public static void StreamWrite()
        {
            string fileName = @"C:\Temp\CSharpAuthors.txt";  
            try  
            {  
                using (StreamWriter writer = new StreamWriter(fileName))  
                {  
                    writer.Write("This file contains C# Corner Authors."); 
                    
                }  
            }  
            catch(Exception exp)  
            {  
                Console.Write(exp.Message);  
            }
        }
    }  
}
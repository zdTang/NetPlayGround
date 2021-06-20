using System;
using System.IO;
using System.Text;

namespace IO
{
    public class TestFile
    {
        public static void Test()
        {
            {

                /*========================
                // File Class
                ==========================*/
                string filePath = @"domoOne.txt";
                //Write
                File.WriteAllText(filePath, "The quick brown fox.");
                // Read
                string testData = File.ReadAllText(filePath);
                string[] testDataLineByLine = File.ReadAllLines(filePath);
                byte[] testDataRawBytes = File.ReadAllBytes(filePath);
                //Write
                string data = "C# Corner MVP & Microsoft MVP;";
                File.WriteAllText(filePath, data);//will cover
                string[] dataArray = { "MCT", "MCPD", "MCTS", "MCSD.NET", "MCAD.NET", "CSM" };
                File.WriteAllLines(filePath, dataArray);
                string dataAppend = "Also Certified from IIT Kharagpur";// will not cover
                File.AppendAllText(filePath, dataAppend);
                string[] otherData = { "Worked with Microsoft", "Lived in USA" };
                File.AppendAllLines(filePath, otherData);   //  append string []
                //var sw=File.AppendText(filePath);
                //sw.Write(data); // StreamWrite doesn't 
                //sw.Write(dataArray);   // doesn't 

                bool doesFileExist = File.Exists(filePath);
                DateTime fileCreatedOn = File.GetCreationTime(filePath);
                
                
                //File.Delete (filePath);
                
                
                /*========================
                // Directory Class
                ==========================*/
                
                var path = Directory.GetCurrentDirectory();
                string sourceDirPath = @"mike";  //  Attention, here is directory
                Directory.CreateDirectory(sourceDirPath);
                {
                    string filePathInsert = @"mike/domoOne.txt";
                    //Write
                    File.WriteAllText(filePathInsert, "The quick brown fox.");
                }
                string sourceSubPath = @"mike/books";  //  Attention, here is directory
                Directory.CreateDirectory(sourceSubPath);
                var isExist = Directory.Exists(sourceDirPath);
                var isExistTwo = Directory.Exists(sourceSubPath);
                string[] subDirectories = Directory.GetDirectories(sourceDirPath);
                string[] files = Directory.GetFiles(sourceDirPath);
                //bool deleteRecursively = true;
                //Directory.Delete(sourceDirPath, deleteRecursively);
                
                
                /*========================
                // DirectoryInfo Class
                ==========================*/   
                
                string DirPath = @"mike\Data";
                DirectoryInfo directory = new DirectoryInfo(DirPath);
                bool directoryExists = directory.Exists;
                if(!directoryExists)
                directory.Create();
                
                //bool deleteRecursively = true;
                //directory.Delete(deleteRecursively);
                string DirMike = @"mike\Data\subData";
                DirectoryInfo directoryMike = new DirectoryInfo(DirMike);
                directoryMike.Create();
                DirectoryInfo[] Directories = directory.GetDirectories();
                FileInfo[] subFiles = directory.GetFiles();
                var isTure =(subFiles == null);
                var look = subFiles.Length;
                Console.WriteLine(look);
                
                /*=========================
                 *    FileInfo and Path
                ==========================*/
                
                
                string thisPath = Path.GetTempFileName();
                var fi1 = new FileInfo(thisPath);

                // Create a file to write to.
                using (StreamWriter sw = fi1.CreateText())
                {
                    sw.WriteLine("Hello");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }	

                // Open the file to read from.
                using (StreamReader sr = fi1.OpenText())
                {
                    var s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                
                
                /*=========================
                 * FileSteam
                 ===========================*/

                string pathFileSteam = @"c:\temp\MyTest.txt";

                // Delete the file if it exists.
                if (File.Exists(pathFileSteam))
                {
                    File.Delete(pathFileSteam);
                }

                //Create the file.
                using (FileStream fs = File.Create(pathFileSteam))
                {
                    AddText(fs, "This is some text");
                    AddText(fs, "This is some more text,");
                    AddText(fs, "\r\nand this is on a new line");
                    AddText(fs, "\r\n\r\nThe following is a subset of characters:\r\n");

                    for (int i=1;i < 120;i++)
                    {
                        AddText(fs, Convert.ToChar(i).ToString());
                    }
                }
                






            }
            void AddText(FileStream fs, string value)
            {
                byte[] info = new UTF8Encoding(true).GetBytes(value);
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestStream
{
    class Adapter
    {
        const string fileName = "d:\\AppSettings.dat";

        public static void Test()

        {
            //第一种ADAPTER
            //继承自 TEXT READER/WRITER：　　STREAM READER/WRITER, STRING READER/WRITER

            #region StreamReader

            //##Stream提供的功能很底层，它面对BYTE,它不清楚这些BYTE是TEXT还是BINARY,因而没法提供ENCODE的功能
            //这些ADAPTER已经知道面对是文本，因而可以取出ENCODE
            //它们都继承自TEXTREADER()

            {

                //try
                //{
                //    //##Stream提供的功能很底层，它面对BYTE,它不清楚这些BYTE是TEXT还是BINARY,因而没法提供ENCODE的功能

                //    //using (var sr = new FileStream("d://trace-2.txt",FileMode.Open))
                //    //{

                //    //    var name = sr.Name;
                //    //    string line;

                //    //}

                //    //## 这个FILE.openText返回的是一个STREAM READER,因而有ENCODE
                //    using (var sr = File.OpenText("d://trace-2.txt"))
                //    {

                //        var name = sr.CurrentEncoding;
                //        string line;

                //    }

                //    // StreamReader默认的就是读一个文本文件，因而它可以取出ENCODE等信息
                //    // Create an instance of StreamReader to read from a file.
                //    // The using statement also closes the StreamReader.
                //    using (StreamReader sr = new StreamReader("d://trace-2.txt"))
                //    {
                //        Encoding ec = sr.CurrentEncoding;
                //        var bs=sr.BaseStream;
                //        string line;
                //        // Read and display lines from the file until the end of
                //        // the file is reached.
                //        while ((line = sr.ReadLine()) != null)
                //        {
                //            Console.WriteLine(line);
                //        }
                //    }
                //}
                //catch (Exception e)
                //{
                //    // Let the user know what went wrong.
                //    Console.WriteLine("The file could not be read:");
                //    Console.WriteLine(e.Message);
                //}
            }

            #endregion

            #region Stream Writer

            {

                //// Get the directories currently on the C drive.
                //DirectoryInfo[] cDirs = new DirectoryInfo(@"c:\").GetDirectories();

                //// Write each directory name to a file.
                //using (StreamWriter sw = new StreamWriter("CDriveDirs.txt"))
                //{
                //    foreach (DirectoryInfo dir in cDirs)
                //    {
                //        sw.WriteLine(dir.Name);
                //    }
                //}

                //// Read and show each line from the file.
                //string line = "";
                //using (StreamReader sr = new StreamReader("CDriveDirs.txt"))
                //{
                //    while ((line = sr.ReadLine()) != null)
                //    {
                //        Console.WriteLine(line);
                //    }
                //}

            }

            #endregion


            //## 专门处理STRING的，貌似READLINE还有点用

            #region String Rader and Writer

            {
                //    string textReaderText = "TextReader is the abstract base " +
                //"class of StreamReader and StringReader, which read " +
                //"characters from streams and strings, respectively.\n\n" +

                //"Create an instance of TextReader to open a text file " +
                //"for reading a specified range of characters, or to " +
                //"create a reader based on an existing stream.\n\n" +

                //"You can also use an instance of TextReader to read " +
                //"text from a custom backing store using the same " +
                //"APIs you would use for a string or a stream.\n\n";

                //    Console.WriteLine("Original text:\n\n{0}", textReaderText);

                //    // From textReaderText, create a continuous paragraph
                //    // with two spaces between each sentence.
                //    string aLine, aParagraph = null;
                //    StringReader strReader = new StringReader(textReaderText);
                //    while (true)
                //    {

                //        aLine = strReader.ReadLine();
                //        if (aLine != null)
                //        {
                //            aParagraph = aParagraph + aLine + " ";
                //        }
                //        else
                //        {
                //            aParagraph = aParagraph + "\n";
                //            break;
                //        }
                //    }
                //    Console.WriteLine("Modified text:\n\n{0}", aParagraph);

                //    // Re-create textReaderText from aParagraph.
                //    int intCharacter;
                //    char convertedCharacter;
                //    StringWriter strWriter = new StringWriter();
                //    strReader = new StringReader(aParagraph);
                //    while (true)
                //    {
                //        intCharacter = strReader.Read();

                //        // Check for the end of the string
                //        // before converting to a character.
                //        if (intCharacter == -1) break;

                //        convertedCharacter = Convert.ToChar(intCharacter);
                //        if (convertedCharacter == '.')
                //        {
                //            strWriter.Write(".\n\n");

                //            // Bypass the spaces between sentences.
                //            strReader.Read();
                //            strReader.Read();
                //        }
                //        else
                //        {
                //            strWriter.Write(convertedCharacter);
                //        }
                //    }
                //    Console.WriteLine("\nOriginal text:\n\n{0}",
                //        strWriter.ToString());
            }

            #endregion

            #region string Reader

            {
                //ReadCharacters();
            }

            #endregion

            #region Encode

            {
                //Console.WriteLine("Default UTF-8 encoding");

                //using (TextWriter w = File.CreateText("but.txt"))    // Use default UTF-8 encoding.
                //    w.WriteLine("but–");                               // Emdash, not the "minus" character

                //using (System.IO.Stream s = File.OpenRead("but.txt"))
                //    for (int b; (b = s.ReadByte()) > -1;)
                //        Console.WriteLine(b);

                //Console.WriteLine("Unicode a.k.a. UTF-16 encoding");

                //using (System.IO.Stream s = File.Create("d://but.txt"))
                //using (TextWriter w = new StreamWriter(s, Encoding.Unicode))
                //{
                //    w.WriteLine("but–");
                //    w.Flush();
                //}



                //foreach (byte b in File.ReadAllBytes("but.txt"))
                //    Console.WriteLine(b);
            }

            #endregion

            //第二种ADAPTER Binary Adapter
            {
                //WriteDefaultValues();
                //DisplayValues();
            }


            {
                using (var w = File.OpenText("but.txt")) ; // Use default UTF-8 encoding.
                using (var y = File.OpenRead("but.txt")) // Use default UTF-8 encoding.
                {

                    


                }
            }
        }

        static async void ReadCharacters()
        {
            StringBuilder stringToRead = new StringBuilder();
            stringToRead.AppendLine("Characters in 1st line to read");
            stringToRead.AppendLine("and 2nd line");
            stringToRead.AppendLine("and the end");

            using (StringReader reader = new StringReader(stringToRead.ToString()))
            {
                string readText = await reader.ReadToEndAsync();
                Console.WriteLine(readText);
            }
        }

        public static void WriteDefaultValues()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write(1.250F);
                writer.Write(@"0123456789=+-");
                writer.Write("唐");
                writer.Write(20);
                writer.Write(true);
                writer.Write(false);
                writer.Flush();
            }
        }

        public static void DisplayValues()
        {
            float aspectRatio;
            string tempDirectory;
            int autoSaveTime;
            bool showStatusBar;

            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    //要按存的顺序才能一个个读出来
                    //STRING类型的，会在前面放一个值，表示长度
                    //CHAR并不是固定BYTE长度，如ASCII都是一个BYTE, “唐”则用了三个BYTE, 但写面写了长度三个BYTE
                    //TRUE 用01H， 而FALSE 用00H
                    aspectRatio = reader.ReadSingle();
                    tempDirectory = reader.ReadString();
                    string tempDirectory2 = reader.ReadString();
                    autoSaveTime = reader.ReadInt32();
                    showStatusBar = reader.ReadBoolean();
                    showStatusBar = reader.ReadBoolean();
                }

                Console.WriteLine("Aspect ratio set to: " + aspectRatio);
                Console.WriteLine("Temp directory is: " + tempDirectory);
                Console.WriteLine("Auto save time set to: " + autoSaveTime);
                Console.WriteLine("Show status bar: " + showStatusBar);
            }
        }




    }
}

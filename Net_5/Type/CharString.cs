using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Type
{
    class CharString
    {
        public static void Test()
        {
            #region Char literal
            {
                // char literals:
                char c = 'A';
                char newLine = '\n';
                // 理解所谓LITERAL,就是所有的SOURCE CODE写在TEXT上，如何用文字表示出不同的TYPE,''这种格式就表示一个CHAR
                // 写SOURCE CODE时，以文本形式书写，但这些东西在内存中就变成了立体的东西
                // System.Char defines a range of static methods for working with characters:
                Console.WriteLine(char.ToUpper('c'));                                   // C
                Console.WriteLine(char.IsWhiteSpace('\t'));                             // True
                Console.WriteLine(char.IsLetter('x'));                                  // True
                Console.WriteLine(char.GetUnicodeCategory('x'));	                    // LowercaseLetter
            }
            #endregion

            #region Construct String
            {
                // String literals:
                string s1 = "Hello";         
                string s2 = @"\\server\fileshare\helloworld.cs";
                string s3 = "First Line\r\nSecond Line";  //回车换行是一对
                string s4 = "First Line Add More\rSecond Line"; // Return如果没有\N,说明光回车不换行，即SECOND LINE又从本行的开头写起
                string s5 = "First Line\nSecond Line";
                Console.WriteLine(s1);
                Console.WriteLine(s2);
                Console.WriteLine(s3);
                Console.WriteLine(s4);
                Console.WriteLine(s5);
                //string s4 = $"\\server\fileshare\helloworld{s1}.cs";

                // To create a repeating sequence of characters you can use string’s constructor:
                Console.Write(new string('*', 10));    // **********
                // STRING->CHAR[], CHAR[]=>STRING 要记住
                // You can also construct a string from a char array. ToCharArray does the reverse:
                char[] ca = "Hello".ToCharArray();
                string s = new string(ca);              // s = "Hello"

            }
            #endregion

            #region String "" and Null
            {
                // An empty string has a length of zero:
                string empty = "";
                Console.WriteLine(empty == "");              // True
                Console.WriteLine(empty == string.Empty);    // True
                Console.WriteLine(empty.Length == 0);        // True

                //Because strings are reference types, they can also be null:
                string nullString = null;
                Console.WriteLine(nullString == null);        // True
                Console.WriteLine(nullString == "");          // False  //null表示啥也没有，""表示STRING对象中DATA是空的，但其它方法都在
                Console.WriteLine(string.IsNullOrEmpty(nullString));    // True
               // Console.WriteLine(nullString.Length == 0);             // NullReferenceException
            }
            #endregion

            #region Play with String
            {
                string str = "abcde";        //可以直接当ARRAY来操作
                char letter = str[1];        // letter == 'b'
                CharEnumerator rator = str.GetEnumerator(); //  string implements IEnumerable,so that have GetEnumerator()

                // string also implements IEnumerable<char>, so you can foreach over its characters:
                foreach (char c in "123") Console.Write(c + ",");    // 1,2,3,

                // The simplest search methods are Contains, StartsWith, and EndsWith:
                Console.WriteLine("quick brown fox".Contains("brown"));    // True
                Console.WriteLine("quick brown fox".EndsWith("fox"));      // True

                // IndexOf returns the first position of a given character or substring:
                Console.WriteLine("abcde".IndexOf("cd"));   // 2
                Console.WriteLine("abcde".IndexOf("xx"));   // -1  其实就是NULL,理解NULL的本质

                // IndexOf is overloaded to accept a startPosition StringComparison enum, which enables case-insensitive searches:
                Console.WriteLine("abcde".IndexOf("CD", StringComparison.CurrentCultureIgnoreCase));    // 2

                // LastIndexOf is like IndexOf, but works backward through the string.
                // IndexOfAny returns the first matching position of any one of a set of characters:
                // 这个方法好，可以用ARRAY放一大堆条件，返回第一个匹配上的INDEX
                Console.WriteLine("ab,cd ef".IndexOfAny(new char[] { ' ', ',' }));       // 2
                Console.WriteLine("pas5w0rd".IndexOfAny("0123456789".ToCharArray()));  // 3

                // LastIndexOfAny does the same in the reverse direction.
            }
            #endregion

            #region Manipulate String
            {
                // Because String is immutable, all the methods below return a new string, leaving the original untouched.

                // Substring extracts a portion of a string:
                string left3 = "12345".Substring(0, 3);     // left3 = "123";第一个数字是起点INDEX,第二个是长度
                string mid3 = "12345".Substring(1, 3);     // mid3 = "234";

                // If you omit the length, you get the remainder of the string:
                string end3 = "12345".Substring(2);        // end3 = "345";

                // Insert and Remove insert or remove characters at a specified position:
                string s1 = "helloworld".Insert(5, ", ");    // s1 = "hello, world"
                string s2 = s1.Remove(5, 2);                 // s2 = "helloworld";

                // PadLeft and PadRight pad a string to a given length with a specified character (or a space if unspecified):
                Console.WriteLine("12345".PadLeft(9, '*'));  // ****12345
                Console.WriteLine("12345".PadLeft(9));       //     12345

                // TrimStart, TrimEnd and Trim remove specified characters (whitespace, by default) from the string:
                Console.WriteLine("  abc \t\r\n ".Trim().Length);   // 3

                // Replace replaces all occurrences of a particular character or substring:
                Console.WriteLine("to be done".Replace(" ", " | "));  // to | be | done
                Console.WriteLine("to be done".Replace(" ", ""));  // tobedone
            }
            #endregion

            #region Split String
            {
                // Split takes a sentence and returns an array of words (default delimiters = whitespace):
                // 非常有用
                string[] words = "The quick brown fox".Split();
 

                // The static Join method does the reverse of Split:
                string together = string.Join(" ", words);
                 // The quick brown fox

                // The static Concat method accepts only a params string array and applies no separator.
                // This is exactly equivalent to the + operator:
                string sentence = string.Concat("The", " quick", " brown", " fox");
                string sameSentence = "The" + " quick" + " brown" + " fox";
            	// The quick brown fox
            }
            #endregion

            #region String Format output
            {
                // When calling String.Format, provide a composite format string followed by each of the embedded variables
                string composite = "It's {0} degrees in {1} on this {2} morning";
                string s = string.Format(composite, 35, "Perth", DateTime.Now.DayOfWeek);

                // The minimum width in a format string is useful for aligning columns.
                // If the value is negative, the data is left-aligned; otherwise, it’s right-aligned:
                composite = "Name={0,-20} Credit Limit={1,15:C}";

                Console.WriteLine(string.Format(composite, "Mary", 500));
                Console.WriteLine(string.Format(composite, "Elizabeth", 20000));

                // The equivalent without using string.Format:
                s = "Name=" + "Mary".PadRight(20) + " Credit Limit=" + 500.ToString("C").PadLeft(15);

            }
            #endregion

            #region StringBuilder
            {
                // Unlike string, StringBuilder is mutable.

                // The following is more efficient than repeatedly concatenating ordinary string types:

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < 50; i++) sb.Append(i + ",");

                // To get the final result, call ToString():
                Console.WriteLine(sb.ToString());

                sb.Remove(0, 60);       // Remove first 50 characters
                sb.Length = 10;         // Truncate to 10 characters
                sb.Replace(",", "+");   // Replace comma with +
                sb.ToString();
            }
            #endregion

            #region Text to Byte[]
            {
                 //理解不同ENCODING占有物BYTE是不同的
                byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes("0123456789");
                byte[] utf16Bytes = System.Text.Encoding.Unicode.GetBytes("0123456789");
                byte[] utf32Bytes = System.Text.Encoding.UTF32.GetBytes("0123456789");

                Console.WriteLine(utf8Bytes.Length);    // 10
                Console.WriteLine(utf16Bytes.Length);   // 20
                Console.WriteLine(utf32Bytes.Length);   // 40
                //从BYTE[] 还原为STRING
                string original1 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                string original2 = System.Text.Encoding.UTF8.GetString(utf16Bytes);
                string original3 = System.Text.Encoding.UTF8.GetString(utf32Bytes);

                string original4 = System.Text.Encoding.Unicode.GetString(utf8Bytes);
                string original5 = System.Text.Encoding.Unicode.GetString(utf16Bytes);
                string original6 = System.Text.Encoding.Unicode.GetString(utf32Bytes);
                
                string original7 = System.Text.Encoding.UTF32.GetString(utf8Bytes);       
                string original8 = System.Text.Encoding.UTF32.GetString(utf16Bytes);
                string original9 = System.Text.Encoding.UTF32.GetString(utf32Bytes);

                Console.WriteLine(original1);          // 0123456789
                Console.WriteLine(original2);          // 0123456789
                Console.WriteLine(original3);          // 0123456789
            }
            #endregion
        }

    }
}

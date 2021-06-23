using System;
using System.IO;
using System.Net;
using System.Text;

namespace TestStream
{
    /*
     *All strings in a .NET Framework program are stored as 16-bit Unicode characters. At times you might need to convert from Unicode to some other character encoding, or from some other character encoding to Unicode. The .NET Framework provides several classes for encoding (converting Unicode characters to a block of bytes in another encoding) and decoding (converting a block of bytes in another encoding to Unicode characters.
 
The System.Text namespace has a number of Encoding implementations: 
ASCIIEncoding class encodes Unicode characters as single 7-bit ASCII characters. This class supports only character values between U+0000 and U+007F.
UnicodeEncoding class encodes each Unicode character as two consecutive bytes. This supports both little-endian (code page 1200) and big-endian (code page 1201) byte orders.
UTF7Encoding class encodes Unicode characters using UTF-7 encoding (UTF-7 stands for UCS Transformation Format, 8-bit form). This supports all Unicode character values and can also be accessed as code page 65000.
UTF8Encoding class encodes Unicode characters using UTF-8 encoding (UTF-8 stands for UCS Transformation Format, 8-bit form). This supports all Unicode character values and can also be accessed as code page 65001. 
     *
     * 
     * 
     * 
     * byte is an unsigned 8-bit integer whose values range from 0 to 255. 
     * char is a 16-bit unicode character type. If you want to represent raw streams of bytes,
     * string is an array of char
     * but stream is byte[]
     */



    public class TestEncoding
    {
        public static void Test()
        {
            string unicodeString = "This string contains the unicode character Pi (\u03a0)";

            // Create two different encodings.
            Encoding ascii = Encoding.ASCII;

            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte array.
            byte[] unicodeBytes = unicode.GetBytes(unicodeString);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
         
            // Convert the new byte[] into a char[] and then into a string.
            char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            string asciiString = new string(asciiChars);

            // Display the strings created before and after the conversion.
            Console.WriteLine("Original string: {0}", unicodeString);
            Console.WriteLine("Ascii converted string: {0}", asciiString);
        }

           
        
    }
}
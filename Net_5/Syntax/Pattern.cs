
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Net_5.Syntax
{
    class Pattern
    {
        public static void Test()
        {
            #region VAR Pattern
            {
                var a = IsJanetOrJohn("Janet");
                var b = IsJanetOrJohn("john");
                // 这是 IS TYPE 的变体，用VAR代替具体的TYPE
                // upper is the intermediate variable,  "var" is the type
                //这里的目是，是为了简化代码，is var upper总是成功的，关注的是用&&以后的部分
                bool IsJanetOrJohn(string name) =>
                    name.ToUpper() is var upper && (upper == "JANET" || upper == "JOHN");
            }


            #endregion

            #region Constant Pattern
            //The constant pattern lets you match directly to a constant and is useful
            //when working with the object type:
            //The following code is same as :
            //    (obj is 3) ===>  obj is int && (int)obj == 3
            {
                Foo(3);

                void Foo(object obj)
                {
                    // C# won’t let you use the == operator, because obj is object.
                    // However, we can use ‘is’
                    if (obj is 3) Console.WriteLine("three");
                }
            }

            #endregion

            #region Relational Pattern

            {
                int x = 150;
                if (x is > 100) Console.WriteLine("x is greater than 100");

                GetWeightCategory(15);
                GetWeightCategory(20);
                GetWeightCategory(25);

                string GetWeightCategory(decimal bmi) => bmi switch
                {
                    < 18.5m => "underweight",
                    < 25m => "normal",
                    < 30m => "overweight",
                    _ => "obese"
                };
            }


            #endregion

            #region Relational Pattern With Object

            {
                object obj = 2m;                  // decimal
                Console.WriteLine(obj is < 3m);  // True
                Console.WriteLine(obj is < 3);   // False
            }


            #endregion

            #region Pattern Combinator

            {
                IsJanetOrJohn("Janet");
                IsVowel('e');
                Between1And9(5);
                IsLetter('!');

                bool IsJanetOrJohn(string name) => name.ToUpper() is "JANET" or "JOHN";

                bool IsVowel(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u';

                bool Between1And9(int n) => n is >= 1 and <= 9;

                bool IsLetter(char c) => c is >= 'a' and <= 'z'
                    or >= 'A' and <= 'Z';
            }


            #endregion

            #region Not Pattern

            {
                object obj = 12;

                if (obj is not string)
                {
                    Console.WriteLine("obj is not a string"); 
                }
            }


            #endregion

            #region Tuple Pattern

            {
                AverageCelsiusTemperature(Season.Spring, true);
                // Return value type : int
                // Keyword:   switch
                int AverageCelsiusTemperature(Season season, bool daytime) =>
                    (season, daytime) switch
                    {
                        (Season.Spring, true) => 20,
                        (Season.Spring, false) => 16,
                        (Season.Summer, true) => 27,
                        (Season.Summer, false) => 22,
                        (Season.Fall, true) => 18,
                        (Season.Fall, false) => 12,
                        (Season.Winter, true) => 10,
                        (Season.Winter, false) => -2,
                        _ => throw new Exception("Unexpected combination")
                    };
            }
            #endregion

            #region Positional Pattern With Record
            // can used on Record TYPE
            {
                    var p = new PointRecord(2, 2);
                    Console.WriteLine(p is (2, 2));                            // True
                    Console.WriteLine(p is (var x, var y) && x == y);   // True

                    Print(new PointRecord(0, 0));
                    Print(new PointRecord(1, 1));

                    string Print(object obj) => obj switch
                    {
                        PointRecord(0, 0) => "Empty point",
                        PointRecord(var x, var y) when x == y => "Diagonal",

                        _ => "Other"
                    };


            }

            #endregion

            #region Positional Pattern with Class
            // 与位置相关的匹配

            {
                Print(new Point(0, 0));
                Print(new Point(1, 1));

                string Print(object obj) => obj switch
                {
                    Point(0, 0) => "Empty point",
                    Point(var x, var y) when x == y => "Diagonal",

                    _ => "Other"
                };

                
            }

            #endregion

            #region Property Pattern

            {
                object obj = "test";
                //if (obj is string s && s.Length == 4)
                if (obj is string { Length: 4 })
                    Console.WriteLine("string with length of 4");
            }

            #endregion

            #region Property Pattern with Switch

            {
                Console.WriteLine(ShouldAllow(new Uri("http://www.linqpad.net")));
                Console.WriteLine(ShouldAllow(new Uri("ftp://ftp.microsoft.com")));
                Console.WriteLine(ShouldAllow(new Uri("tcp:foo.database.windows.net")));

                bool ShouldAllow(Uri uri) => uri switch
                {
                    { Scheme: "http", Port: 80 } => true,
                    { Scheme: "https", Port: 443 } => true,
                    { Scheme: "ftp", Port: 21 } => true,
                    { IsLoopback: true } => true,
                    _ => false
                };
            }

            #endregion

            #region Property Pattern with Switch Nested

            {
                Console.WriteLine(ShouldAllow(new Uri("http://www.linqpad.net")));
                Console.WriteLine(ShouldAllow(new Uri("ftp://ftp.microsoft.com")));
                Console.WriteLine(ShouldAllow(new Uri("tcp:foo.database.windows.net")));

                bool ShouldAllow(Uri uri) => uri switch
                {
                    { Scheme: { Length: 4 }, Port: 80 } => true,
                    _ => false
                };
            }

            #endregion

            #region Property Pattern with Switch and Relational Pattern

            {
                Console.WriteLine(ShouldAllow(new Uri("http://www.linqpad.net")));
                Console.WriteLine(ShouldAllow(new Uri("ftp://ftp.microsoft.com")));
                Console.WriteLine(ShouldAllow(new Uri("tcp:foo.database.windows.net")));

                bool ShouldAllow(Uri uri) => uri switch
                {
                    { Host: { Length: < 1000 }, Port: > 0 } => true,
                    _ => false
                };
            }

            #endregion

            #region Property Pattern with WHEN Clause

            {
                Console.WriteLine(ShouldAllow(new Uri("http://www.linqpad.net")));
                Console.WriteLine(ShouldAllow(new Uri("ftp://ftp.microsoft.com")));
                Console.WriteLine(ShouldAllow(new Uri("tcp:foo.database.windows.net")));

                bool ShouldAllow(Uri uri) => uri switch
                {
                    { Scheme: "http" } when string.IsNullOrWhiteSpace(uri.Query) => true,
                    _ => false
                };
            }

            #endregion

            #region Property Pattern with Type Pattern

            {
                Console.WriteLine(ShouldAllow(new Uri("http://www.linqpad.net")));
                Console.WriteLine(ShouldAllow(new Uri("ftp://ftp.microsoft.com")));
                Console.WriteLine(ShouldAllow(new Uri("tcp:foo.database.windows.net")));

                bool ShouldAllow(object uri) => uri switch
                {
                    Uri { Scheme: "http", Port: 80 } httpUri => httpUri.Host.Length < 1000,
                    Uri { Scheme: "https", Port: 443 } => true,
                    Uri { Scheme: "ftp", Port: 21 } => true,
                    Uri { IsLoopback: true } => true,
                    _ => false
                };
            }

            #endregion

            #region Property Pattern with Type Pattern and WHEN

            {
                Console.WriteLine(ShouldAllow(new Uri("http://www.linqpad.net")));
                Console.WriteLine(ShouldAllow(new Uri("ftp://ftp.microsoft.com")));
                Console.WriteLine(ShouldAllow(new Uri("tcp:foo.database.windows.net")));

                bool ShouldAllow(object uri) => uri switch
                {
                    Uri { Scheme: "http", Port: 80 } httpUri
                        when httpUri.Host.Length < 1000 => true,
                    Uri { Scheme: "https", Port: 443 } => true,
                    Uri { Scheme: "ftp", Port: 21 } => true,
                    Uri { IsLoopback: true } => true,
                    _ => false
                };
            }

            #endregion

            #region Property Pattern with Property Variable

            {
                Console.WriteLine(ShouldAllow(new Uri("http://www.linqpad.net")));
                Console.WriteLine(ShouldAllow(new Uri("ftp://ftp.microsoft.com")));
                Console.WriteLine(ShouldAllow(new Uri("tcp:foo.database.windows.net")));

                bool ShouldAllow(Uri uri) => uri switch
                {
                    { Scheme: "http", Port: 80, Host: var host } => host.Length < 1000, // variable Here
                    { Scheme: "https", Port: 443 } => true,
                    { Scheme: "ftp", Port: 21 } => true,
                    { IsLoopback: true } => true,
                    _ => false
                };
            }

            #endregion
        }
        
    }
    enum Season { Spring, Summer, Fall, Winter };
    record PointRecord(int X, int Y);
    class Point
    {
        public readonly int X, Y;

        public Point(int x, int y) => (X, Y) = (x, y);

        // Here's our deconstructor:
        public void Deconstruct(out int x, out int y)
        {
            x = X; y = Y;
        }
    }
}

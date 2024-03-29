﻿using System;
using System.Numerics;

namespace Net_5.testType
{
    class BasicTypes
    {
        public static void Test()
        {
            Console.WriteLine("***** Fun with Basic Data Types *****\n");
            LocalVarDeclarations();
            DefaultDeclarations();
            NewingDataTypes();
            ObjectFunctionality();
            DataTypeFunctionality();
            CharFunctionality();
            ParseFromStrings();
            ParseFromStringsWithTryParse();
            UseDatesAndTimes();
            UseBigInteger();
            DigitSeparators();
            BinaryLiterals();
            Console.ReadLine();
        }

        #region Local variable declarations

        static void LocalVarDeclarations()
        {
            Console.WriteLine("=> Data Declarations:");
            // Local variables are declared and initialized as follows:
            // dataType varName = initialValue;
            int myInt = 0;

            string myString;
            myString = "This is my character data";

            // Declare 3 bools on a single line.
            bool b1 = true, b2 = false, b3 = b1;

            // Very verbose manner in which to declare a bool.
            //Bool is just System.Boolean shortened ??
            //https://stackoverflow.com/questions/134746/what-is-the-difference-between-bool-and-boolean-types-in-c-sharp
            System.Boolean b4 = false;

            Console.WriteLine("Your data: {0}, {1}, {2}, {3}, {4}, {5}",
                myInt, myString, b1, b2, b3, b4);
            Console.WriteLine();
        }

        static void DefaultDeclarations()
        {
            //Console.WriteLine("=> Default Declarations:");
            //int myInt = default;
        }

        //https://stackoverflow.com/questions/9207488/what-does-the-keyword-new-do-to-a-struct-in-c
        // In a struct, the new keyword is needlessly confusing.It doesn't do anything. It's just required if you want to use the constructor. It does not perform a new.
        //The usual meaning of new is to allocate permanent storage(on the heap.) A language like C++ allows new myObject() or just myObject(). Both call the same constructor.But the former creates a new object and returns a pointer.The latter merely creates a temp.Any struct or class can use either. new is a choice, and it means something.
        //C# doesn't give you a choice. Classes are always in the heap, and structs are always on the stack. It isn't possible to perform a real new on a struct. Experienced C# programmers are used to this. When they see ms = new MyStruct(); they know to ignore the new as just syntax. They know it's acting like ms = MyStruct(), which merely assigns to an existing object.

        // 注意，对于NEW，这里只是个形式，并不是要在HEAP上分配空间给STRUCT
        static void NewingDataTypes()
        {
            //Console.WriteLine("=> Using new to create variables:");
            //bool b = new bool(); // Set to false.
            //bool bb = default;
            //int i = new int(); // Set to 0.
            //double d = new double(); // Set to 0.
            //DateTime dt = new DateTime(); // Set to 1/1/0001 12:00:00 AM
            //Console.WriteLine("{0}, {1}, {2}, {3}", b, i, d, dt);
            //Console.WriteLine();
        }

        #endregion

        #region Test Object functionality

        static void ObjectFunctionality()
        {
        //    Console.WriteLine("=> System.Object Functionality:");
        //    // A C# int is really a shorthand for System.Int32.
        //    // which inherits the following members from System.Object.
        //    Console.WriteLine("12.GetHashCode() = {0}", 12.GetHashCode());
        //    Console.WriteLine("12.Equals(23) = {0}", 12.Equals(23));
        //    Console.WriteLine("12.ToString() = {0}", 12.ToString());
        //    Console.WriteLine("12.GetType() = {0}", 12.GetType());
        //    Console.WriteLine();
        }

        #endregion

        #region Test data type functionality

        static void DataTypeFunctionality()
        {
            //Console.WriteLine("=> Data type Functionality:");
            //Console.WriteLine("Max of int: {0}", int.MaxValue);
            //Console.WriteLine("Min of int: {0}", int.MinValue);
            //Console.WriteLine("Max of double: {0}", double.MaxValue);
            //Console.WriteLine("Min of double: {0}", double.MinValue);
            //Console.WriteLine("double.Epsilon: {0}", double.Epsilon);
            //Console.WriteLine("double.PositiveInfinity: {0}",
            //    double.PositiveInfinity);
            //Console.WriteLine("double.NegativeInfinity: {0}",
            //    double.NegativeInfinity);
            //Console.WriteLine("bool.FalseString: {0}", bool.FalseString);  // False
            //Console.WriteLine("bool.TrueString: {0}", bool.TrueString);    // True
            //Console.WriteLine();
        }

        #endregion

        #region Test char data type

        static void CharFunctionality()
        {
            //Console.WriteLine("=> char type Functionality:");
            //char myChar = 'a';
            //Console.WriteLine("char.IsDigit('a'): {0}", char.IsDigit(myChar));
            //Console.WriteLine("char.IsLetter('a'): {0}", char.IsLetter(myChar));
            //Console.WriteLine("char.IsWhiteSpace('Hello There', 5): {0}",
            //    char.IsWhiteSpace("Hello There", 5));  // 用的INDEX
            //Console.WriteLine("char.IsWhiteSpace('Hello There', 6): {0}",
            //    char.IsWhiteSpace("Hello There", 6));
            //Console.WriteLine("char.IsPunctuation('?'): {0}",
            //    char.IsPunctuation('?'));
            //Console.WriteLine();
        }

        #endregion

        #region Parsing data
        // FROM STRING TO CORRESPONDING TYPE
        // 所谓PARSE, 应该是从文本（或字面量），即STRING转为相应的类型？
        // 要理解CS源文件就是一个文本，是一个STRING
        static void ParseFromStrings()
        {
            //Console.WriteLine("=> Data type parsing:");
            //bool b = bool.Parse("True");
            //Console.WriteLine("Value of b: {0}", b);
            //double d = double.Parse("99.884");
            //Console.WriteLine("Value of d: {0}", d);
            //int i = int.Parse("8");
            //Console.WriteLine("Value of i: {0}", i);
            //char c = Char.Parse("w");
            //Console.WriteLine("Value of c: {0}", c);
            //Console.WriteLine();

            //bool test = true;       // literal 并不一定是STRING
        }

        static void ParseFromStringsWithTryParse()
        {
            //Console.WriteLine("=> Data type parsing with TryParse:");
            //if (bool.TryParse("True", out bool b))
            //{
            //    Console.WriteLine("Value of b: {0}", b); // 一般OUT 参数都是先定义，这样也可以，注意用法
            //}
            //else
            //{
            //    Console.WriteLine("Default value of b: {0}", b);
            //}

            //string value = "Hello";
            //if (double.TryParse(value, out double d))
            //{
            //    Console.WriteLine("Value of d: {0}", d);
            //}
            //else
            //{
            //    Console.WriteLine(
            //        "Failed to convert the input ({0}) to a double and the variable was assigned the default {1}",
            //        value, d);
            //}

            //Console.WriteLine();
        }

        #endregion

        #region Work with dates / times

        static void UseDatesAndTimes()
        {
            //Console.WriteLine("=> Dates and Times:");
            //// This constructor takes (year, month, day)
            //DateTime dt = new DateTime(2015, 10, 17);

            //// What day of the month is this?
            //Console.WriteLine("The day of {0} is {1}", dt.Date, dt.DayOfWeek);
            //dt = dt.AddMonths(2); // Month is now December.
            //Console.WriteLine("Daylight savings: {0}", dt.IsDaylightSavingTime());

            //// This constructor takes (hours, minutes, seconds)
            //TimeSpan ts = new TimeSpan(4, 30, 0);
            //Console.WriteLine(ts);

            //// Subtract 15 minutes from the current TimeSpan and
            //// print the result.
            //Console.WriteLine(ts.Subtract(new TimeSpan(0, 15, 0)));
            //Console.WriteLine();
        }

        #endregion

        #region Use BigInteger

        static void UseBigInteger()
        {
            //Console.WriteLine("=> Use BigInteger:");
            //BigInteger biggy = BigInteger.Parse("9999999999999999999999999999999999999999999999");
            //Console.WriteLine("Value of biggy is {0}", biggy);
            //Console.WriteLine("Is biggy an even value?: {0}", biggy.IsEven);
            //Console.WriteLine("Is biggy a power of two?: {0}", biggy.IsPowerOfTwo);
            //BigInteger reallyBig = BigInteger.Multiply(biggy,
            //    BigInteger.Parse("8888888888888888888888888888888888888888888"));
            //BigInteger reallyBig2 = biggy * reallyBig;

            //Console.WriteLine("Value of reallyBig is {0}", reallyBig);
        }

        #endregion

        #region Use Digit Separators

        static void DigitSeparators()
        {
            //Console.WriteLine("=> Use Digit Separators:");
            //Console.Write("Integer:");
            //Console.WriteLine(123_456);
            //Console.Write("Long:");
            //Console.WriteLine(123_456_789L);
            //Console.Write("Float:");
            //Console.WriteLine(123_456.1234F);
            //Console.Write("Double:");
            //Console.WriteLine(123_456.12);
            //Console.Write("Decimal:");
            //Console.WriteLine(123_456.12M);
            ////Updated in 7.2, Hex can begin with _
            //Console.Write("Hex:");
            //Console.WriteLine(0x_00_00_FF);
        }

        #endregion

        #region Use Binary Literals

        private static void BinaryLiterals()
        {
            ////Updated in 7.2, Binary can begin with _
            //Console.WriteLine("=> Use Binary Literals:");
            //Console.WriteLine("Sixteen: {0}", 0b_0001_0000);
            //Console.WriteLine("Thirty Two: {0}", 0b_0010_0000);
            //Console.WriteLine("Sixty Four: {0}", 0b_0100_0000);
        }

        #endregion
    }
}
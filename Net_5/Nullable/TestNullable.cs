using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Nullable
{
    class TestNullable
    {
        public static void Test()
        {
            // To represent null in a value type, you must use a special construct called a nullable type:
            {
                int? i = null; // Nullable Type
                Console.WriteLine(i == null); // True
                //正真的REFERENCE类型，当为NULL时，就无法读取它的方法或属性
                //但是NULLABLE类型是可以取HASVALUE()的
                var a = i.HasValue;
                var aa = i ?? 100; // if i==null, aa=100
                var aaa = (i ??= 100).ToString(); //if i==null, aa=100
                // ??
                // IF Left operand isn't null, then return the left operand
                // if left operand is null, then evaluate the right side operand
                // ??  
                //var b = i ?? i.Value;
                //var k = i?.Value;
                //var b = i.Value;
                int? j = 25;
                var c = j.HasValue;
                var d = j.Value;
                string s = "mike";
                int? ss = s?.Length;

            }
            // Equivalent to:
            {
                Nullable<int> i = new Nullable<int>();
                Console.WriteLine(!i.HasValue); // True
            }
            {
                // The conversion from T to T? is implicit, and from T? to T is explicit:

                int? x = 5; // implicit
                int y = (int) x; // explicit
            }
            {
                // When T? is boxed, the boxed value on the heap contains T, not T?.
                // C# also permits the unboxing of nullable types with the as operator:

                object o = "string";
                int? x = o as int?;
                Console.WriteLine(x.HasValue); // False
            }
            {
                // Despite the Nullable<T> struct not defining operators such as <, >, or even ==, the 
                // following code compiles and executes correctly, thanks to operator lifting:

                int? x = 5;
                int? y = 10;
                {
                    bool b = x < y; // true
                }
                // The above line is equivalent to:
                {
                    bool b = (x.HasValue && y.HasValue) ? (x.Value < y.Value) : false;

                }
            }
            {
                //对于NULLABLE，NULL表示这个STRUCT的VALUE属性上没有值
                int? x = 5;
                int? y = null;
                {
                    int? c = x + y;
                }
                // Translation: 本质
                {
                    int? c = (x.HasValue && y.HasValue)
                        ? (int?) (x.Value + y.Value)
                        : null;
                }
            }
            {
               //!!!!!! HERE IS THE SWITCH
                #nullable enable     // Attention, 这里是开关

                string s1 = null; // Generates a compiler warning!
                string? s2 = null; // OK: s2 is nullable reference type
            }





        }
    }


        class Foo
        {
            string x;     // Generates a warning
        }
    }

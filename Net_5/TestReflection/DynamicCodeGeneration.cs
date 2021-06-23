using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestReflection
{
    class DynamicCodeGeneration
    {
        public static void Test()
        {
            #region Generating IL with DynamicMethod

            {
                //// DynamicMethod : MethodInfo 继承关系
                //// 搞明白这几个参数的意思，最后一个是OWNER,即这个方法隶属于哪一个CLASS
                //// 这表示要为NEWTEST这个CLASS做一个叫FOO的方法，它无返回值，无参
                //var dynMeth = new DynamicMethod("Foo", null, null, typeof(NewTest));
                //ILGenerator gen = dynMeth.GetILGenerator();    // 生成一个ILGenerator
                //gen.EmitWriteLine("Hello world");
                ///*这一句是SHORTCUT,与下面三行是相同的,注意OPCODES这个CLASS,它用不同FIELD表示了不同的操作动作
                //MethodInfo writeLineStr = typeof(Console).GetMethod("WriteLine",new Type[] { typeof(string) }); //拿到参数是STRING的那个WRITELINE()
                //gen.Emit(OpCodes.Ldstr, "Hello world");      // Load a string
                //gen.Emit(OpCodes.Call, writeLineStr);        // Call a method
                //*/
                //gen.Emit(OpCodes.Ret);       // 表示RETURN
                //dynMeth.Invoke(null, null);                    // Hello world
            }

            #endregion

            #region Generating IL with DynamicMethod-Non public  access
            //这里的点在于，这个METHOD是PRIVATE的，只有在CLASS内部才能CALL得到
            //因而要生成一个属于这个CLASS的方法，才CALL它
            {
                ////为这个CLASS动态成生一个METHOD,因为只有这个CLASS的METHOD，才能CALL它内部的PRIVATE METHOD
                //var dynMeth = new DynamicMethod("Foo", null, null, typeof(TestTwo));
                //ILGenerator gen = dynMeth.GetILGenerator();
                //// 拿出CLASS中的PRIVATE METHOD,要给一些参数才可以
                //MethodInfo privateMethod = typeof(TestTwo).GetMethod("HelloWorld", BindingFlags.Static | BindingFlags.NonPublic);

                //gen.Emit(OpCodes.Call, privateMethod);     // Call HelloWorld
                //gen.Emit(OpCodes.Ret);

                //dynMeth.Invoke(null, null);                // Hello world
            }

            #endregion

            #region Evaluation Stack


            // ONE PARAMETER
            {
                ///* void 是一个STRUCT,这就是它的定义
                //   public struct Void
                //    {
                //    }
                // */
                //var dynMeth = new DynamicMethod("Foo", null, null, typeof(void));
                //ILGenerator gen = dynMeth.GetILGenerator();
                ////拿到参数为INT的WRITELINE
                //MethodInfo writeLineInt = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) });

                //// The Ldc* op-codes load numeric literals of various types and sizes.

                //gen.Emit(OpCodes.Ldc_I4, 123);        // Push a 4-byte integer onto stack
                //gen.Emit(OpCodes.Call, writeLineInt);

                //gen.Emit(OpCodes.Ret);
                //dynMeth.Invoke(null, null);           // 123
            }

            // TWO PARAMETERS
            {
                //var dynMeth = new DynamicMethod("Foo", null, null, typeof(void));
                //ILGenerator gen = dynMeth.GetILGenerator();
                //MethodInfo writeLineInt = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) });

                //// Calculate 2 + 2

                //gen.Emit(OpCodes.Ldc_I4, 2);           // Push a 4-byte integer, value=2
                //gen.Emit(OpCodes.Ldc_I4, 2);           // Push a 4-byte integer, value=2
                //gen.Emit(OpCodes.Add);                 // Add the result together
                //gen.Emit(OpCodes.Call, writeLineInt);

                //gen.Emit(OpCodes.Ret);
                //dynMeth.Invoke(null, null);
            }
            //  多个参数的操作
            //  这里要了解STACK中数的操作，ADD,DIVIDE等操作数首先要把之前的两个值从STACK中取出来，计算之后，再把结果PUCH进去
            {
                //var dynMeth = new DynamicMethod("Foo", null, null, typeof(void));
                //ILGenerator gen = dynMeth.GetILGenerator();
                //MethodInfo writeLineInt = typeof(Console).GetMethod("WriteLine",
                //    new Type[] { typeof(int) });

                //// Calculate 10 / 2 + 1:

                //gen.Emit(OpCodes.Ldc_I4, 10);
                //gen.Emit(OpCodes.Ldc_I4, 2);
                //gen.Emit(OpCodes.Div);
                //gen.Emit(OpCodes.Ldc_I4, 1);
                //gen.Emit(OpCodes.Add);
                //gen.Emit(OpCodes.Call, writeLineInt);

                //// Here's another way to do the same thing:

                //gen.Emit(OpCodes.Ldc_I4, 1);
                //gen.Emit(OpCodes.Ldc_I4, 10);
                //gen.Emit(OpCodes.Ldc_I4, 2);
                //gen.Emit(OpCodes.Div);
                //gen.Emit(OpCodes.Add);
                //gen.Emit(OpCodes.Call, writeLineInt);


                //gen.Emit(OpCodes.Ret);
                //dynMeth.Invoke(null, null);
            }
            #endregion

            #region Passing Arguments to a DynamicMethod

            {
                //DynamicMethod dynMeth = new DynamicMethod("Foo",
                //    typeof(int),                                         // Return type = int
                //    new[] { typeof(int), typeof(int) },      // Parameter types = int, int
                //    typeof(void)); //这里，OWNER弄个VOID是什么意思？

                //ILGenerator gen = dynMeth.GetILGenerator();

                //gen.Emit(OpCodes.Ldarg_0);      // Push first arg onto eval stack
                //gen.Emit(OpCodes.Ldarg_1);      // Push second arg onto eval stack
                //gen.Emit(OpCodes.Add);          // Add them together (result on stack)
                //gen.Emit(OpCodes.Ret);          // Return with stack having 1 value

                //int result = (int)dynMeth.Invoke(null, new object[] { 3, 4 });   // 7
                //result.Dump();

                //// If you need to invoke the method repeatedly, here's an optimized solution:
                //var func = (Func<int, int, int>)dynMeth.CreateDelegate(typeof(Func<int, int, int>));
                //result = func(3, 4);      // 7
                //result.Dump();
            }

            #endregion

            #region Generate Local Variable

            {
                ////int x = 6;
                ////int y = 7;
                ////x *= y;
                ////Console.WriteLine (x);

                //var dynMeth = new DynamicMethod("Test", null, null, typeof(void));
                //ILGenerator gen = dynMeth.GetILGenerator();

                //LocalBuilder localX = gen.DeclareLocal(typeof(int));    // Declare x
                //LocalBuilder localY = gen.DeclareLocal(typeof(int));    // Declare y
                ////下面这四步不太明白，为什么不能直接把值给LOCAL呢？ 而是都要经过EVAL STACK
                //gen.Emit(OpCodes.Ldc_I4, 6);        // Push literal 6 onto eval stack
                //gen.Emit(OpCodes.Stloc, localX);    // Store in localX
                //gen.Emit(OpCodes.Ldc_I4, 7);        // Push literal 7 onto eval stack
                //gen.Emit(OpCodes.Stloc, localY);    // Store in localY

                //gen.Emit(OpCodes.Ldloc, localX);    // Push localX onto eval stack
                //gen.Emit(OpCodes.Ldloc, localY);    // Push localY onto eval stack
                //gen.Emit(OpCodes.Mul);              // Multiply values together
                //gen.Emit(OpCodes.Stloc, localX);    // Store the result to localX

                //gen.EmitWriteLine(localX);          // Write the value of localX
                //gen.Emit(OpCodes.Ret);

                //dynMeth.Invoke(null, null);         // 42

            }

            #endregion

            #region Branching
            //这是用ASSEMBLY的语句进行循环，跳转

            {
                ////int x = 5;
                ////while (x <= 10) Console.WriteLine (x++);

                //var dynMeth = new DynamicMethod("Test", null, null, typeof(void));
                //ILGenerator gen = dynMeth.GetILGenerator();

                //Label startLoop = gen.DefineLabel();                  // Declare labels
                //Label endLoop = gen.DefineLabel();

                //LocalBuilder x = gen.DeclareLocal(typeof(int));     // int x
                //gen.Emit(OpCodes.Ldc_I4, 5);                         //
                //gen.Emit(OpCodes.Stloc, x);                          // x = 5
                //gen.MarkLabel(startLoop);
                //{
                //    gen.Emit(OpCodes.Ldc_I4, 10);              // Load 10 onto eval stack
                //    gen.Emit(OpCodes.Ldloc, x);                // Load x onto eval stack

                //    gen.Emit(OpCodes.Blt, endLoop);            // if (x > 10) goto endLoop

                //    gen.EmitWriteLine(x);                      // Console.WriteLine (x)

                //    gen.Emit(OpCodes.Ldloc, x);                // Load x onto eval stack
                //    gen.Emit(OpCodes.Ldc_I4, 1);               // Load 1 onto the stack
                //    gen.Emit(OpCodes.Add);                     // Add them together
                //    gen.Emit(OpCodes.Stloc, x);                // Save result back to x

                //    gen.Emit(OpCodes.Br, startLoop);           // return to start of loop
                //}
                //gen.MarkLabel(endLoop);

                //gen.Emit(OpCodes.Ret);

                //dynMeth.Invoke(null, null);
            }

            #endregion

            #region Instantiating Objects and Calling Instance Methods

            {
                //var dynMeth = new DynamicMethod("Test", null, null, typeof(void));
                //ILGenerator gen = dynMeth.GetILGenerator();

                //ConstructorInfo ci = typeof(StringBuilder).GetConstructor(new Type[0]);
                //gen.Emit(OpCodes.Newobj, ci);
                //// CALL了StringBuilder的一个MaxCapacity，用GET取出值
                //gen.Emit(OpCodes.Callvirt, typeof(StringBuilder).GetProperty("MaxCapacity").GetGetMethod());
                //// CALL了Console的一个WriteLine方法，那个参数为INT的
                //gen.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine",new[] { typeof(int) }));
                //gen.Emit(OpCodes.Ret);

                //dynMeth.Invoke(null, null);              // 2147483647
            }

            #endregion

            #region Appending to a StringBuilder

            {
                //var dynMeth = new DynamicMethod("Test", null, null, typeof(void));
                //ILGenerator gen = dynMeth.GetILGenerator();

                //// We will call:   new StringBuilder ("Hello", 1000)

                //ConstructorInfo ci = typeof(StringBuilder).GetConstructor(new[] { typeof(string), typeof(int) });

                //gen.Emit(OpCodes.Ldstr, "Hello");   // Load a string onto the eval stack
                //gen.Emit(OpCodes.Ldc_I4, 1000);     // Load an int onto the eval stack
                //gen.Emit(OpCodes.Newobj, ci);       // Construct the StringBuilder

                //Type[] strT = { typeof(string) };
                //gen.Emit(OpCodes.Ldstr, ", world!");
                //gen.Emit(OpCodes.Call, typeof(StringBuilder).GetMethod("Append", strT));
                //gen.Emit(OpCodes.Callvirt, typeof(object).GetMethod("ToString"));
                //gen.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", strT));
                //gen.Emit(OpCodes.Ret);

                //dynMeth.Invoke(null, null);        // Hello, world!
            }

            #endregion

            #region Exception Handler

            {
                //var dynMeth = new DynamicMethod("Test", null, null, typeof(void));
                //ILGenerator gen = dynMeth.GetILGenerator();

                //// try                               { throw new NotSupportedException(); }
                //// catch (NotSupportedException ex)  { Console.WriteLine (ex.Message);    }
                //// finally                           { Console.WriteLine ("Finally");     }

                //MethodInfo getMessageProp = typeof(NotSupportedException)
                //    .GetProperty("Message").GetGetMethod();

                //MethodInfo writeLineString = typeof(Console).GetMethod("WriteLine",
                //    new[] { typeof(object) });
                //gen.BeginExceptionBlock();
                //{
                //    ConstructorInfo ci = typeof(NotSupportedException).GetConstructor(
                //        new Type[0]);
                //    gen.Emit(OpCodes.Newobj, ci);
                //    gen.Emit(OpCodes.Throw);
                //}
                //gen.BeginCatchBlock(typeof(NotSupportedException));
                //{
                //    gen.Emit(OpCodes.Callvirt, getMessageProp);
                //    gen.Emit(OpCodes.Call, writeLineString);
                //}
                //gen.BeginFinallyBlock();
                //{
                //    gen.EmitWriteLine("Finally");
                //}
                //gen.EndExceptionBlock();

                //gen.Emit(OpCodes.Ret);

                //dynMeth.Invoke(null, null);        // Hello, world!

            }

            #endregion
        }
    }

    public class NewTest
    {
    }
    public class TestTwo
    {
        static void HelloWorld()       // private method, yet we can call it
        {
            Console.WriteLine("Hello world");
        }
    }
}

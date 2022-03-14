using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
//using LINQPad;

namespace Net_5.TestReflection
{
    class EmmittingAssembliesAndTypes
    {
        // TODO: REFLECTION 后面一连几段太偏，有空看一下
        // emit  TYPE MEMBERS
        // emit  generic methods and types
        // awkward emission target
        // parsing IL

        public static void Test()
        {
            #region
           // EmmittingAssembliesAndTypes

            {
                // what is Assembly:
                // assembly: A configured set of loadable code modules and other resources that together implement a unit of functionality.

                // 第一步， ASSEMBLY NAME
                AssemblyName aname = new AssemblyName("MyDynamicAssembly");

                // 第二步， 在名字的基本上，建ASSEMBLY BUILDER
                AssemblyBuilder assemBuilder = AssemblyBuilder.DefineDynamicAssembly(aname, AssemblyBuilderAccess.Run);
                /*
                  Run = 1,
                  RunAndCollect = 9,
                 */
                // 第三步， 在ASSEMBLE BUILDER的基本上，建MODULE BUILDER
                ModuleBuilder modBuilder = assemBuilder.DefineDynamicModule("DynModule");

                // 第四步， 在MODULE BUILDER的基本上，建TYPE BUILDER

                TypeBuilder tb = modBuilder.DefineType("Widget", TypeAttributes.Public);

                // 第五步， 在TYPE BUILDER的基本上，建METHOD BUILDER

                // 此时，这个TYPE BUILDER 已经可以建与TYPE相关的几乎任何东西

                MethodBuilder methBuilder = tb.DefineMethod("SayHello", MethodAttributes.Public, null, null);

                // 第六步， 在METHOD BUILDER的基本上，建ILGenerator
                ILGenerator gen = methBuilder.GetILGenerator();

                // 第七步， 由ILGenerator 来 EMIT 
                gen.EmitWriteLine("Hello world");
                gen.Emit(OpCodes.Ret);

                // Create the type, finalizing its definition:
                Type t = tb.CreateType();

     
                // Once the type is created, we use ordinary reflection to inspect
                // and perform dynamic binding:
                // 从一个TYPE实例中，能否INFER出它的实例的类型，它可能是一个CLASS
                object o = Activator.CreateInstance(t); // 建一个t TYPE的OBJECT


                t.GetMethod("SayHello").Invoke(o, null);        // Hello world
            }

            #endregion

            #region emmit Methods

            {
                // public static double SquareRoot (double value) => Math.Sqrt (value);

                AssemblyName aname = new AssemblyName("MyEmissions");

                AssemblyBuilder assemBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    aname, AssemblyBuilderAccess.Run);

                ModuleBuilder modBuilder = assemBuilder.DefineDynamicModule("MainModule");

                TypeBuilder tb = modBuilder.DefineType("Widget", TypeAttributes.Public);

                MethodBuilder mb = tb.DefineMethod("SquareRoot",
                    MethodAttributes.Static | MethodAttributes.Public,
                    CallingConventions.Standard,
                    typeof(double),                     // Return type
                    new[] { typeof(double) });        // Parameter types

                mb.DefineParameter(1, ParameterAttributes.None, "value");  // Assign name

                ILGenerator gen = mb.GetILGenerator();
                gen.Emit(OpCodes.Ldarg_0);                                // Load 1st arg
                gen.Emit(OpCodes.Call, typeof(Math).GetMethod("Sqrt"));
                gen.Emit(OpCodes.Ret);

                Type realType = tb.CreateType();
                double x = (double)tb.GetMethod("SquareRoot").Invoke(null,
                    new object[] { 10.0 });
                Console.WriteLine(x);   // 3.16227766016838

                // LINQPad can disassemble methods for you:
                //tb.GetMethod("SquareRoot").Disassemble().Dump("LINQPad disassembly");
            }

            #endregion

            //  Type and Instance of TYPE
            //  可以把TYPE实例看作是TYPE的给系统用的注册表
            {  
                // Person is a type
                // P是PERSON这个TYPE的实例
                // 任何TYPE,都可以生成一个TYPE实例
                // t is not a type, t 是  PERSON的  TYPE实例
                // 这里，PERSON是TYPE名，可以用来表示一个VALUE
                // 而T是PERSON这个TYPE的内容，不是TYPE名字
                // T是TYPE 类型的实例，以PERSON为参数
                
                Type t=typeof(Person);
                Person p = new Person();
                
                TypeInfo ti = t.GetTypeInfo();
                Type tt=ti.AsType();
            }
        }



    }

    class Person
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestReflection
{
    [Serializable, Obsolete]
    //这个SERIALIZABLE是个pseudo-custom attribute,
    //它在内部被优化，其实是改了一个BIT-MAPPED ATTRIBUTE
    //但它在GETCUSTOMATTRIBUTES（）时，也会被返回
    //因而可以不理会这个pseudo-custom attribute 与 custom attribute的区别
    class WorkingWithAttribute
    {
        public static void Test()
        {
            #region Bit-mapped attributes
            //这个ATTRIBUTE是个总合集，一下子可以把很多属性全拿出来
            //其它的TYPE中的ISPUBLIC等属性，不过是从它身上分解出来的
            {
  //              /*  TypeAttributes是个ENUM
  //                 [Flags]
  //                public enum TypeAttributes
  //                {
  //                  AnsiClass = 0,
  //                  AutoLayout = 0,
  //                  Class = 0,
  //                  NotPublic = 0,
  //                  Public = 1,
  //                  NestedPublic = 2,
  //                  NestedPrivate = NestedPublic | Public,
  //                  NestedFamily = 4,
  //                  NestedAssembly = NestedFamily | Public,
  //                  NestedFamANDAssem = NestedFamily | NestedPublic,
  //                  NestedFamORAssem = NestedFamANDAssem | NestedAssembly,
  //                  VisibilityMask = NestedFamORAssem,
  //                  SequentialLayout = 8,
  //                  ExplicitLayout = 16,
  //                  LayoutMask = ExplicitLayout | SequentialLayout,
  //                  ClassSemanticsMask = 32,
  //                  Interface = ClassSemanticsMask,
  //                  Abstract = 128,
  //                  Sealed = 256,
  //                  SpecialName = 1024,
  //                  RTSpecialName = 2048,
  //                  Import = 4096,
  //                  Serializable = 8192,
  //                  WindowsRuntime = 16384,
  //                  UnicodeClass = 65536,
  //                  AutoClass = 131072,
  //                  CustomFormatClass = AutoClass | UnicodeClass,
  //                  StringFormatMask = CustomFormatClass,
  //                  HasSecurity = 262144,
  //                  ReservedMask = HasSecurity | RTSpecialName,
  //                  BeforeFieldInit = 1048576,
  //                  CustomFormatMask = 12582912,
  //                }
  //               */
  //              TypeAttributes ta = typeof(Console).Attributes;
  //              ta.Dump();

  //              /*  MethodAttributes是个ENUM
  //               *  [Flags]
  //                public enum MethodAttributes
  //                {
  //                  PrivateScope = 0,
  //                  ReuseSlot = 0,
  //                  Private = 1,
  //                  FamANDAssem = 2,
  //                  Assembly = FamANDAssem | Private,
  //                  Family = 4,
  //                  FamORAssem = Family | Private,
  //                  Public = Family | FamANDAssem,
  //                  MemberAccessMask = Public | FamORAssem,
  //                  UnmanagedExport = 8,
  //                  Static = 16,
  //                  Final = 32,
  //                  Virtual = 64,
  //                  HideBySig = 128,
  //                  NewSlot = 256,
  //                  VtableLayoutMask = NewSlot,
  //                  CheckAccessOnOverride = 512,
  //                  Abstract = 1024,
  //                  SpecialName = 2048,
  //                  RTSpecialName = 4096,
  //                  PinvokeImpl = 8192,
  //                  HasSecurity = 16384,
  //                  RequireSecObject = 32768,
  //                  ReservedMask = RequireSecObject | HasSecurity | RTSpecialName,
  //}
  //               */
  //              MethodBase mb = MethodInfo.GetCurrentMethod(); //这个可以拿到当前正在执行的方法METHOD,可以好好利用一下 
  //              MethodAttributes ma = MethodInfo.GetCurrentMethod().Attributes;
  //              ma.Dump();
            }

            #endregion

            #region Define Own Attribute
            // 我们平时谈论的ATTRIBUTE就是CUSTOM ATTRIBUTE
            // This instructs the compiler to incorporate an instance of ObsoleteAttribute into
            // the metadata for Foo, which then can be reflected at runtime by calling GetCustom
            // Attributes on a Type or MemberInfo object. Reflection and Me
            {
                //foreach (MethodInfo mi in typeof(Foo).GetMethods())
                //{
                //    //注意，ATTRIBUTE有这样一个STATIC 方法 GetCustomAttribute
                //    //从每一个METHODINFO中取出我提前定义的TESTATTRIBUTE
                //    TestAttribute att = (TestAttribute)Attribute.GetCustomAttribute(mi, typeof(TestAttribute));

                //    if (att != null)
                //        Console.WriteLine("Method {0} will be tested; reps={1}; msg={2}",
                //            mi.Name, att.Repetitions, att.FailureMessage);
                //}
            }

            // unit test example
            // 研究这个与ATTRIBUTE配合，进行UNIT TEST的例子
            {
                //foreach (MethodInfo mi in typeof(Foo).GetMethods())
                //{
                //    TestAttribute att = (TestAttribute)Attribute.GetCustomAttribute(mi, typeof(TestAttribute));

                //    if (att != null)
                //        for (int i = 0; i < att.Repetitions; i++)
                //            try
                //            {
                //                mi.Invoke(new Foo(), null);    // Call method with no arguments
                //                $"Successfully called {mi.Name}".Dump();
                //            }
                //            catch (Exception ex)       // Wrap exception in att.FailureMessage
                //            {
                //                throw new Exception("Error: " + att.FailureMessage, ex);
                //            }
                //}
            }

            // 注意，这个WorkingWithAttribute前面加了几个CUSTOM ATTRIBUTE
            {
                //object[] atts = Attribute.GetCustomAttributes(typeof(WorkingWithAttribute));
                //foreach (object att in atts) Console.WriteLine(att);
            }

            #endregion

        }
    }
    // 这个是所谓的CUSTOM ATTRIBUTE
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestAttribute : Attribute
    {
        public int Repetitions;
        public string FailureMessage;

        public TestAttribute() : this(1) { }
        public TestAttribute(int repetitions) { Repetitions = repetitions; }
    }

    class Foo
    {
        [Test]
        public void Method1() { }

        [Test(20)]
        public void Method2() { }

        [Test(20, FailureMessage = "Debugging Time!")]
        public void Method3() { }
    }


}

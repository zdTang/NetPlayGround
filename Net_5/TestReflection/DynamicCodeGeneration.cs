using System;
using System.Collections.Generic;
using System.Linq;
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
                // DynamicMethod : MethodInfo 继承关系
                // 搞明白这几个参数的意思，最后一个是OWNER,即这个方法隶属于哪一个CLASS
                var dynMeth = new DynamicMethod("Foo", null, null, typeof(NewTest));
                ILGenerator gen = dynMeth.GetILGenerator();    // 生成一个ILGenerator
                gen.EmitWriteLine("Hello world");
                gen.Emit(OpCodes.Ret);
                dynMeth.Invoke(null, null);                    // Hello world
            }

            #endregion
        }
    }

    public class NewTest
    {
    }
}

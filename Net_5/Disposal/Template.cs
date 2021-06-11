using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Disposal
{
    class Template
    {
        public static void Test()
        {

            //using (Calculator calculator = new Calculator())
            //{
            //    Console.WriteLine($"120 / 15 = {calculator.Divide(120, 15)}");
            //    Console.WriteLine("Program finishing");
            //}
            //没有用USING,我希望用FINILIZE()来清理，但是DECONSTRUCTOR不运行，不清楚为什么
            //在MICROSOFT C# STEP BY STEP 中的同样例子去可以用
            //有空研究一下
            Calculator calculator = new Calculator();
            Console.WriteLine($"120 / 15 = {calculator.Divide(120, 15)}");
            Console.WriteLine("Program finishing");
            
        }



    }

    //这个模板来自MICROSOFT C# STEP BY STEP，有详细解释
    //NUBSHELL中也有讲解
    //在DESTRUCTOR中调用DISPOSE()
    //这样做的好处是BACKUP.
    //一旦写了DISPOSE（）却不被人调用
    class Calculator : IDisposable
    {
        private bool disposed = false;

        public Calculator()
        {
            Console.WriteLine("Calculator being created");
        }

        ~Calculator()
        {
            Console.WriteLine("Calculator being finalized");
            this.Dispose();
        }

        public int Divide(int first, int second)
        {
            return first / second;
        }

        public void Dispose()
        {
            // 如果用了USING,或主动调用DISPOSE()时，会走上面IF语名之中，全部清理，无论MANAGED 还是NON-MANAGED

            if (!this.disposed)
            {
                Console.WriteLine("Calculator being disposed");
                //managed code put here
            } //如果在DECONSTRUCTOR中调用的DISPOSE,则只走这一块，只清理UN - MANAGED CODE
            //   unmanaged code put here

            this.disposed = true;
            GC.SuppressFinalize(this); //如果DISPOSE之前走了一次，这里就设置不要让FINALIZE再搞事情了
        }
    }

}
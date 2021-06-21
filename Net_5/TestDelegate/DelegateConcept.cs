using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestDelegate
{
    //好好记住DELEGATE的定义方式，这是定义了一种TYPE，如果定义一个CLASS一样
    //它可以被用在实例的前面，表示TYPE
    public delegate void MyDelegate(int num);    //Define a customized delegate
    public delegate void D(int num);             //Define a customized delegate
    public delegate void DD1(C c, string s);
    public delegate void DD2(string s);
    public delegate void DD3();
    class DelegateConcept
    {
        public static void Test()
        {
            //use the Delegate
            // X,X2,X3都属于D TYPE,但却是不同的DELEGATE,规定了不同的LOGIC
            D X = (int num) => { };
            D X2 = (int num) => { Console.WriteLine((num * num).ToString()); };
            D X3 = (int num) => { Console.WriteLine((num*num*num ).ToString()); };
            X(100);
            X2(100);
            X3(100);


            //  Action=== no return value
            Action a = new Action(() => { Console.WriteLine("hi"); });
            a.Invoke();
            //Action b = new Action(PrintOut);   //  works
            Action b = PrintOut;
            b.Invoke();

            Action<int> c = new Action<int>(PrintOutWithPara);
            c.Invoke(100);

            Action<int> d = PrintOutWithPara;
            d.Invoke(100);
            // Must have return value
            Func<int> funcOne = new Func<int>(() => { return 1; });
            Func<int> funTwo = new Func<int>(() => { Console.WriteLine("func"); return 1; });
            Func<int, string> funThree = myInt => "my Result" + myInt.ToString();
            string result = funThree.Invoke(100);
            Console.WriteLine(result);


            //  Customized DELEGATE
            MyDelegate mydelegate = myInt => Console.WriteLine(myInt.ToString());
            MyDelegate mydelegateTwo = givenInt => Console.WriteLine("this is another one=={0}", givenInt);
            //mydelegate.Invoke(23);

            /// Past delegate as paramater
            //PassDelegate(mydelegate, 99);
            //PassDelegate(mydelegateTwo, 99);
            //PassDelegate(intPara => { }, 99);

            // MULTI-BROADCASTING 

            Console.WriteLine("===Test MULTICASTING====");
            mydelegate += mydelegateTwo;
            mydelegate += PrintOutWithPara;
            mydelegate += PrintOutWithParaTwo;
            mydelegate.Invoke(88);

        }

        static void PrintOut()
        {
            Console.WriteLine("how are you.");
        }


        static void PrintOutWithPara(int num)
        {
            Console.WriteLine("how are you.---{0}", num);
        }
        static void PrintOutWithParaTwo(int num)
        {
            Console.WriteLine("how are you.TWO---{0}", num);
        }

        /// Past a Delegate as Parameter
        static void PassDelegate(MyDelegate a, int i)
        {
            a.Invoke(i);
        }
    }
}

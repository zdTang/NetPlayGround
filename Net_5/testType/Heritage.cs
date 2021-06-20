using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.testType
{
    class Heritage
    {
        public static void Test()
        {

        }

    }

    abstract class GrandPa
    {
        public abstract void Speak();
        protected string Name;
    }
    abstract class OtherDad:GrandPa
    {
        //只可以用OVERRIDE,不可以用NEW, 因为父ABSTRACT只是一个空的
        //这于接口的继承不同，接口只可以用NEW,不可以用OVERRIDE
        public override void Speak() 
        { }
        protected string Name;
    }

}

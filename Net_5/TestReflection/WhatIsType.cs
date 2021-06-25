using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestReflection
{
    class WhatIsType
    {
        // 这里要别开TYPE，class, interface,delegate之类
        // CLASS, INTERFACE,delegate 本身不是TYPE，是关键字，是用来定义TYPE的
        // 而TYPE不是个关键字，它本身与STRING,OBJECT一样，就是一个CLASS型的TYPE
        // 由CLASS, INTERFACE关键字定义的ITOOL,MyTool就是TYPE了
        // TYPE不是一个关键字，它就是一个用CLASS定义的TYPE, 与STRING,MyTool一样是个CLASS
        // 只不过TYPE是个内定的CLASS
        public static void Test()
        {
            throw new MyException();

            Type t = typeof(MyTool);     // mytool 是个用关键字CLASS定义的TYPE
            Type i = typeof(ITool);      // ITool  是个用关键字INTERFACE定义的TYPE
            Type type = typeof(Type);    // TYPE   是个用关键字CLASS定义的TYPE
            //Type d = typeof(delegate); // delegate小定的，是关键字，不是个TYPE,它用定义TYPE
            
            Type e = typeof(EatWord);   // 这是一个TYPE
            Type d = typeof(Delegate);   // 大写的Delegate 是个TYPE
            Type action = typeof(Action);   // 这是一个TYPE
            Type func = typeof(Func<>);   // 这是一个TYPE


            //anonymous Type
            var v = new { Amount = 108, Message = "Hello" };
            Type ano = v.GetType();

            t.Dump();
            i.Dump();

            //  Enum type

            Animal a = Animal.Bird;
            Type en = typeof(Animal);
            Type enu = a.GetType();
            Type enn = typeof(Enum);    //  首字母大写，这是个TYPE
            //var eeee = new Enum();
            var tool = new MyTool();
            //var abTool = new MyAbstractTool();  //抽象TYPE只能继承，不能实例？？因为方法不完整？？



        }
    }

    delegate void EatWord(string s);
    interface ITool
    {

    }

    class MyTool : ITool
    {

    }

    abstract class MyAbstractTool : ITool
    {

    }

    enum Animal
    {
        Cat,
        Dog,
        Bird
    }

    class MyException : Exception
    {
        public override string Message=>"wrong";
    }
}

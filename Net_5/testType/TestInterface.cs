using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.testType
{
    class TestInterface
    {
        public static void Test()
        {
            #region Interface Inherit
            // 对于接口的EXTEND, 没有被NEW的会被子接口继承
            // 被NEW的老方法，只有截上老接口的帽子才可以看到
            // 方法的返回值不属于签名，当新接口要改变老接口中方法的返回值类型，相当于要改这个方法，要用NEW
            // 方法的参数数量，参数类型是签名，当改变签名，相当于增加了一个方法，不需要加NEW
            // 如果接口是继承自父的，则只能由父来显式实现（这个方法不会出现在子接口的定义之中）
            // 如果父级的一个方法被子级NEW了，则这个被NEW的方法必须有一个父级的显式实现（否则就会丢失这个父级的配方）
            // 如果截上父类的帽子，但该方法没有显式继承，也看不到
            // 可以理解为技术的传承，每一代有自己的方法，后代可能摒弃前一代的做法
            // 但是我们可以显式的采用老一代的工艺，就要显式说明用哪一代的做法
            // 改变返回值可以算作改变传统做法，但改变参数个数不算，被列为添加一种做法
            // 由于是接口，没有方法体， 所谓的NEW只是在于新接口改变了老接口工艺中返回值的类型
            // 以本例而言，GRANDPA, DAD, SON, DECEDENT共四级，前三级是接口，第四级是CLASS,在IMPLEMENT这三级接口中的新旧方法
            // 作为实施接口的CLASS,有隐式的方法实现（一级级迭代，经过逐代NEW, 目前默认使用的配方）
            // 作为实施接口的CLASS,有显式的方法实现（迭代中被NEW掉了的老配方, 需要显式指明谁的配方才能使用的--被NEW的老配方必须显式实现！！）
            {
                var ob = new Decendent();
                ob.Redo();
                ob.Undo();
                ob.Undo(1);
                ob.Undo(1,2);
                //子接口的实例调用方法
                Decendent obb = new Decendent();
                obb.Redo();                         //子接口改写父接口，NEW出来的
                obb.Undo();                         //子接口改写父接口，NEW的
                obb.Undo(1,2);                  //子接口新加的
                obb.Undo(1);                       //父接口中的，父接口自己加的

                //实例戴上最底层接口（子接口），如同实例，可以拿到所有隐式，即默认方法，这是那个有返回值的REDO
                ISon opp = new Decendent();
                opp.Redo();                     //  SON接口看到的是SON的方法实现（如果SON有显式实现）
                opp.Undo();                     //  SON接口看到的是SON的方法实现（如果SON有显式实现）
                opp.Undo(1,2);              //  SON接口看到的是SON的方法实现（如果SON有显式实现）
                opp.Undo(1);//来自父接口       //  这个方法是继承自DAD的，不能被SON接口显式实现，SON接口可以看到并调用它
                                                //  如果DECENDENT TYPE中没有显式实施DAD的这个方法，就会绑定CLASS中的隐式方法
                                                //  理论上说，这个DAD方法被传承下来，不需要再显式写一个DAD的方法实现
                                                //  但如果显式实施了DAD的这个方法，则这里运行的是DAD的方法

                // OBJECT戴上父接口
                IDad omm = new Decendent();
                omm.Redo();                     // DAD接口看到的是DAD的方法实现（如果DAD有显式实现）
                omm.Undo();                     // DAD接口看到的是DAD的方法实现（如果DAD有显式实现）
                omm.Undo(1);                  // DAD接口看到的是DAD的方法实现（如果DAD有显式实现）
  
                // OBJECT戴上父接口
                IGrandpa okk = new Decendent();
                okk.Undo();                     // Grandpa接口看到的是GRAMDPA的方法实现，如果GRANDPA有显式实现

            }

            #endregion
        }
        //在TWO中测试INTERFACE的显式和隐式两种继承
        public static void TestTwo()
        {
            //见后面，DADTWO是显式实现接口，此时，要调用方法，必须有接口的帽子
            DadTwo dad = new DadTwo();  // DADTWO是接口显式实现

            /* 这里用实例竟然调不出一个方法  !!!!*/

            //dad.                      

            /*戴上接口就可以调用*/
            IDad dad2 = new DadTwo();     //但调不出GRANDPA的方法
            dad2.Redo();         //void
           int a = dad2.Undo();         // return int
            dad2.Undo(1);      // void
            
            IGrandpa dad3 = new DadTwo();
            dad3.Undo();         // void

        }
    }


    public interface IGrandpa

    { void Undo(); }

    public interface IDad : IGrandpa
    {
        void Redo();
        new int Undo();  //虽然返回值变化，但函数签名没变，这个需要NEW
        void Undo(int a); // 这个是父接口自己扩展的内容，有参数，签名动了，属于新方法，与爷爷接口无冲突
    }
    // 测试OVERRIDE,不可以用在接口上
    // OVERRIDE只是用在VIRTUAL 方法上
    public interface IStepDad : IGrandpa
    {
        void Redo();
        //override int Undo();  //OVERRIDE是不可以用在INTERFACE上的，只有NEW才可以
                                //对于ABSTRACT元素，只可以OVERRIDE，而不可以NEW,这是为什么？
        void Undo(int a); // 
    }

    public interface ISon : IDad, IGrandpa
    {   //由于INTERFACE没有BODY,如果NEW之后,返回值类型不变，则没有任何意义，跟原来的一样
        new string Undo();    //这是重写,
        //int Undo();  //这个算重写了，虽然返回值改变，但返回值不属于签名，因而它不算一个新的方法
        void Undo(int a, int b); // 这个不算重写，参数变化，算是子接口加的个新内容
        new int Redo();   // 这个改变了返回值，但不算新方法，是重写，要需NEW
    }

    class Decendent : ISon
    {
        
        //继承链的默认方法，可有可无？下面两个隐式方法即使删除也不会报错，因为已经被显示实现了
        public string Undo()  // 这个系统选择显式实现，这个隐式实现是我自己加的
        {
            Console.WriteLine("I am class Decendent--UNdo");
            return "ok";
        }
        public int Redo()// 这个系统选择显式实现，这个隐式实现是我自己加的
        {
            Console.WriteLine("I am class Decendent--Redo");
            return 1;
        }
        public void Undo(int a, int b)
        {
            Console.WriteLine("I am class Decendent--undo, two paramerters");
        }
      
        public void Undo(int a)
        {
            Console.WriteLine("I am class Decendent--undo, one parameters");
        }

        //=========================================//

        void ISon.Undo(int a, int b)
        {
            Console.WriteLine("I Belong to  Son--undo, two paramerters");
        }
        int ISon.Redo()//这个系统选择显示实现，这是系统加的
        {
            Console.WriteLine("I Belong to Son--Redo");
            return 1;
        }
        // UNDO,儿子自己的手法
        string ISon.Undo()// 这个系统选择显示实现，这是系统加的
        {
            Console.WriteLine("I Belong to Son--UNdo");
            return "ok";
        }
        //void ISon.Undo(int a) // 这是父的接口，只能由父来显式实现
        //{

        //}
        // REDO, 爸爸自己的手法


        //=========================================//
        //DAD的这个方法被SON给NEW了，因而必须显式实现，否出出错
        void IDad.Redo()
        {
            Console.WriteLine("I Belong to Dad--Redo"); ;
        }
        // 这个没有参数的UNDO,是爸爸NEW爷爷的，现在又被儿子NEW了
        // 因而这个方法必须显示写出，否则出错
        // 对于UNDO，爸爸的不同操作手法是，有返回值INT
        int IDad.Undo()
        {
            Console.WriteLine("I Belong to Dad--undo, Return an Int");
            return 1;
        }
        //这个爸爸的方法没有被儿子NEW,因而传到了CLASS中
        //不用戴爸爸的眼镜也能找到，大家用的就是它
        //因为这个方法被沿用了，即使不用爸爸来显式实现，也不出错
        //如果加上爸爸显式实现，好处是当戴爸爸眼镜看时，找的是爸爸的版本
        void IDad.Undo(int a)
        {
            Console.WriteLine("I Belong to Dad--undo,  Need an Int");

        }


        //=========================================//
        // UNDO, 爷爷的手法
        // 这个方法被后代NEW,因而必须有一个显式方法记录下它的古老配方
        void IGrandpa.Undo()
        {
            Console.WriteLine("I Belong to grandPA--undo,");
        }
    }

    interface IDemo<T> : IEnumerable<T>, IEnumerable
    {

    }
    //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-interfaces
    //class Demo<T> : IDemo<T> // 如果接口不CLOSE,用个T表示，则这个CLASS必须也用T,因为要从CLASS中得知T的TYPE
    class Demo : IDemo<int> // CLASS不必是范型的，此时的接口CLOSE,是INT型的
                            // 如果CLASS不是GENERIC的，则接口也不是
                            // 接口中的T必须来自前面CLASS或STRUCT的定义
    {
        public IEnumerator<int> GetEnumerator()//隐式实现
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()// 这个被GENERIC的NEW了，因而需要显式实现
        {
            throw new NotImplementedException();
        }
    }

    //隐式实现接口的例子
    //除了被直接接口NEW掉的方法需要加一个老接口头，其它不用，实例可以直接调用
    class Dad : IDad
    {
        public void Redo()  //0层接口
        {
            throw new NotImplementedException();
        }

        public int Undo()//0层接口
        {
            throw new NotImplementedException();
        }

        public void Undo(int a)//0层接口
        {
            throw new NotImplementedException();
        }

        void IGrandpa.Undo()//0层接口NEW掉的上层接口
        {
            throw new NotImplementedException();
        }
    }

    //显式实现接口的情况，全部有接口头儿
    class DadTwo : IDad
    {
        void IDad.Redo()
        {
            throw new NotImplementedException();
        }

        int IDad.Undo()
        {
            throw new NotImplementedException();
        }

        void IDad.Undo(int a)
        {
            throw new NotImplementedException();
        }

        void IGrandpa.Undo()
        {
            throw new NotImplementedException();
        }
    }




}

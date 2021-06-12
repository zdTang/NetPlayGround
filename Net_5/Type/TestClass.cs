using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Type
{
    class TestClass
    {
    }

    //这个例是就是CLASS中调用自己，熟悉这种用法
    //理解，CLASS是个TYPE
    //真正的每个实例，只是包括了FIELD和PROPERTY（STATE）部分,不要多考虑METHOD,那些只构成LOGIC部分
    //METHOD只是构成了这个TYPE内部的一个逻辑
    //对于实例METHOD,实例自己就是一个参数，需将自己传入（PYTHON中就很直接）
    //对于STATIC METHOD, 没有实例参数
    class Papa
    {
        static private int _age=100;

        private Papa(int age)
        { _age = age;}
        public static Papa Default = new Papa(_age);
    }
}

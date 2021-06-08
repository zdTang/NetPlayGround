using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Collection
{
    class CustomizableCollectionNew
    {
        public static void Test()
        {
            ZooNew zoo = new ZooNew();
            zoo.Animals.Add(new AnimalNew("Kangaroo", 10));
            zoo.Animals.Add(new AnimalNew("Mr Sea Lion", 20));
            foreach (AnimalNew a in zoo.Animals) Console.WriteLine(a.Name);
        }
    }


    public class AnimalNew
    {
        public string Name;
        public int Popularity;

        public ZooNew Zoo { get; internal set; }   //放在哪个ZOO

        public AnimalNew(string name, int popularity)
        {
            Name = name; Popularity = popularity;
        }
    }
    //
    public class AnimalCollectionNew : Collection<AnimalNew>
    {
        ZooNew zoo;
        public AnimalCollectionNew(ZooNew zoo) { this.zoo = zoo; }

        protected override void InsertItem(int index, AnimalNew item)
        {
            base.InsertItem(index, item);  //先调用老方法
            item.Zoo = zoo;  //这里额外做了事情，实例把自己的信息交给了传入的参数
                             //这种使用非常诡异，外面传入一个参数，这个参数可以传入信息
                             //也可以利用这个参数把内部信息带出去
                             //这里传入一个动物，同时把动物园的名称也绑定给动物了
        }
        protected override void SetItem(int index, AnimalNew item)
        {
            base.SetItem(index, item);  //先调用老方法
            item.Zoo = zoo;             //再做点额外事情
        }
        protected override void RemoveItem(int index)
        {
            this[index].Zoo = null;  //额外做了事情，自己的信息传给了参数
            base.RemoveItem(index);  //处理完自己信息，再调用老方法
        }
        protected override void ClearItems()
        {
            foreach (AnimalNew a in this) a.Zoo = null;
            base.ClearItems();
        }
    }

    public class ZooNew
    {
        public readonly AnimalCollectionNew Animals;
        public ZooNew() { Animals = new AnimalCollectionNew(this); }
    }

}

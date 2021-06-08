using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Collection
{
    class CustomizableCollection
    {
        public static void Test()
        {

            #region ObjectModel Collection
            //这里提供一个框架如何来订制一个COLLECTION
            //就是继承Collection<T>这个CLASS
            //这个Collection<T>就是在LIST<T>的基础上，对于一些涉及改动内容的METHOD，又提供了WRAPER方法
            //即额外提供了一些VIRTUAL方法，默认情况下，这些VIRTUAL方法只是调用了以前的普通METHOD
            //但由于是VIRTUAL方法，因而USER可以OVERRIDE它们，使得在调用以前的普通METHOD之前，
            //做一些我们想做的事情，从而达到定制的目的
            //如果不OVERRIDE这些VIRTUAL方法，则用起来与普通的COLLECTION没有区别

            {
                Zoo zoo = new Zoo();
            var age = zoo.age;
            var name = zoo.School;
            zoo.Animals.Add(new Animal("Kangaroo", 10));
            zoo.Animals.Add(new Animal("Mr Sea Lion", 20));
            foreach (Animal a in zoo.Animals) Console.WriteLine(a.Name);
            }

            #endregion
        }

    }

    public class Animal
    {
        public string Name;
        public int Popularity;

        public Animal(string name, int popularity)
        {
            Name = name; Popularity = popularity;
        }
    }

    public class AnimalCollection : Collection<Animal>
    {
        // AnimalCollection is already a fully functioning list of animals.
        // No extra code is required
        //Collection<>实施了IList<>,增加了一些VIRTUAL方法如果不需要额外功能，直接用就行
        //如果需要订制，则可以重写这些VIRTUAL方法【OVERRIDE】
        //理解，这些VIRTUAL提供了一个GATEWAY,或缓冲，让USER在普通操作之前，做一些自己的动作
        //这里如里不OVERRIDE这些VIRTUAL METHOD,用起来就是一个LIST<ANIMAL>
        //The virtual methods provide the gateway by which you can “hook in” to change or
        //enhance the list’s normal behavior.The protected Items property allows the imple‐
        //menter to directly access the “inner list”—this is used to make changes internally
        //without the virtual methods firing.
        /*
            protected virtual void ClearItems() => this.items.Clear();
            protected virtual void InsertItem(int index, T item) => this.items.Insert(index, item);
            protected virtual void RemoveItem(int index) => this.items.RemoveAt(index);
            protected virtual void SetItem(int index, T item) => this.items[index] = item;
            protected IList<T> Items { get; }//可以被子类访问
         */
    }

    public class Zoo   // The class that will expose AnimalCollection.
    {                  // This would typically have additional members.
        //这是一个FIELD,在CONSTRUCTOR之前INIT
        public readonly AnimalCollection Animals = new AnimalCollection();
        public string School;          
        public int age { set; get; }   // 所有字段都会在CONSTRUCTOR之间被初始化
                                       // AGE是个PROPERTY,它的BACKING FIELD也会被初始化为DEFAULT
    }

}

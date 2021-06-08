using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Collection
{
    namespace KeyedCollectionSpace
    {
       // 这是框架提供的可以定制KEY-VALUE COLLECTION的方案
        class KeyedCollection
        {
        }

        public class Animal
        {
            string name;
            public string Name
            {
                get { return name; }
                set
                {
                    if (Zoo != null) Zoo.Animals.NotifyNameChange(this, value);
                    name = value;
                }
            }
            public int Popularity;
            public Zoo Zoo { get; internal set; }

            public Animal(string name, int popularity)
            {
                Name = name; Popularity = popularity;
            }
        }
        //类似的套路，提供一个WRAPPER方法（VIRTUAL)
        //如果不OVERRIDE,就是默认功能
        //想定制，就OVERRIDE,然后利用BASE来调用功能
        public class AnimalCollection : KeyedCollection<string, Animal>
        {
            Zoo zoo;
            public AnimalCollection(Zoo zoo) { this.zoo = zoo; }

            internal void NotifyNameChange(Animal a, string newName)
            {
                this.ChangeItemKey(a, newName);
            }

            protected override string GetKeyForItem(Animal item)
            {
                return item.Name;
            }

            protected override void InsertItem(int index, Animal item)
            {
                base.InsertItem(index, item);
                item.Zoo = zoo;
            }
            protected override void SetItem(int index, Animal item)
            {
                base.SetItem(index, item);
                item.Zoo = zoo;
            }
            protected override void RemoveItem(int index)
            {
                this[index].Zoo = null;
                base.RemoveItem(index);
            }
            protected override void ClearItems()
            {
                foreach (Animal a in this) a.Zoo = null;
                base.ClearItems();
            }
        }

        public class Zoo
        {
            public readonly AnimalCollection Animals;
            public Zoo() { Animals = new AnimalCollection(this); }
        }


	}


}

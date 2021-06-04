using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Net_5.TestAttribute
{
    class TestAttribute
    {
        public static void Test()
        {
            var a =typeof(Foo);
            var b = new Foo();
            var c = b.GetType();
            PropertyInfo[] properties=a.GetProperties();
            var d=properties[0];
            IEnumerable<Attribute> att=d.GetCustomAttributes();
            var attB = d.Attributes;
            FieldInfo[] f = a.GetFields();
           // var ff = f[0];
            Foo.Boo();

            
            // TEST caller's attribute
            {
                var foo = new NewFoo();
                // once property has been changed, the Event will  be fired and this Subscriber will work
                foo.PropertyChanged += (sender, args) => Console.WriteLine(args.PropertyName);
                foo.CustomerName = "asdf";
            }



        } 

    }

    //   Caller's Attribute
    class Foo
    {
        [field: NonSerialized]
        public int MyProperty { get; set; }

        public static void Boo(
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int lineNumber = 0)
        {
            Console.WriteLine(memberName);
            Console.WriteLine(filePath);
            Console.WriteLine(lineNumber);
        }
    }

    //Once a property changed, will trigger a Notification
    //这个例子示犯了使用CALL ATTRIBUTE，来做一个EVENT,当PROPERTY变动时，进行提醒

    public class NewFoo : INotifyPropertyChanged
    {
        // This EVENT is from  Interface
        // An  EVENT should be a DELEGATE
        public event PropertyChangedEventHandler PropertyChanged;

        // Define a Broadcaster: Prepare Data for Launching the Event
        // propertyName is a parameter, it comes from Caller, and will be wrapped in EventArgs
        // PropertyChangedEventArgs is a CLASS derived from EventArgs
        void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        string customerName;
        public string CustomerName
        {
            get => customerName;
            set
            {
                if (value == customerName) return;
                customerName = value;
                RaisePropertyChanged();    // Fire the Event
                // The compiler converts the above line to:
                // RaisePropertyChanged ("CustomerName");
            }
        }
    }
}

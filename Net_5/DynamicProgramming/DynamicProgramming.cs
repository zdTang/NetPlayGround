using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Net_5.DynamicProgramming
{
    class DynamicProgramming
    {
        public static void Test()
        {
            #region Numeric type unification
            {
                //int x = 3, y = 5;  // use static T Mean<T>(T x, T y)
                //double x = 4, y = 5;//  use static double Mean(double x, double y)
                dynamic x = 4, y = 7;  // use T Mean<T>(T x, T y)
                //string s = Mean(3, 5); // Runtime error!
                Console.WriteLine(Mean(x, y));
            }
            #endregion

            #region Visitor Pattern
            // TODO: 有空仔细看这两个例子
            {
                var cust = new Customer { FirstName = "Joe", LastName = "Bloggs", CreditLimit = 123 };
                cust.Friends.Add(new Employee { FirstName = "Sue", LastName = "Brown", Salary = 50000 });
                new ToXElementPersonVisitor().DynamicVisit(cust).Dump();
            }
            //这个例子在NAMESPECE TestTwo 中
            {
                var cust = new TestTwo.Customer { FirstName = "Joe", LastName = "Bloggs", CreditLimit = 123 };
                cust.Friends.Add(new TestTwo.Employee { FirstName = "Sue", LastName = "Brown", Salary = 50000 });
                new TestTwo.ToXElementPersonVisitor().DynamicVisit(cust).Dump();

            }
            #endregion




        }

        static dynamic Mean(dynamic x, dynamic y) => (x + y) / 2;

        static T Mean<T>(T x, T y)
        {
            "Dynamic".Dump();
            dynamic result = ((dynamic)x + y) / 2;
            return (T)result;
        }

        static double Mean(double x, double y)
        {
            "Static".Dump();
            return (x + y) / 2;
        }
    }

    class ToXElementPersonVisitor
    {
        public XElement DynamicVisit(Person p)
        {
            return Visit((dynamic)p);
        }

        XElement Visit(Person p)
        {
            return new XElement("Person",
                new XAttribute("Type", p.GetType().Name),
                new XElement("FirstName", p.FirstName),
                new XElement("LastName", p.LastName),
                p.Friends.Select(f => DynamicVisit(f))
            );
        }

        XElement Visit(Customer c)   // Specialized logic for customers
        {
            XElement xe = Visit((Person)c);   // Call "base" method
            xe.Add(new XElement("CreditLimit", c.CreditLimit));
            return xe;
        }

        XElement Visit(Employee e)   // Specialized logic for employees
        {
            XElement xe = Visit((Person)e);   // Call "base" method
            xe.Add(new XElement("Salary", e.Salary));
            return xe;
        }
    }


    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // The Friends collection may contain Customers & Employees:
        public readonly IList<Person> Friends = new Collection<Person>();
    }

    class Customer : Person { public decimal CreditLimit { get; set; } }
    class Employee : Person { public decimal Salary { get; set; } }

    namespace TestTwo
    {
        abstract class PersonVisitor<T>
        {
            public T DynamicVisit(Person p) { return Visit((dynamic)p); }

            protected abstract T Visit(Person p);
            protected virtual T Visit(Customer c) { return Visit((Person)c); }
            protected virtual T Visit(Employee e) { return Visit((Person)e); }
        }

        class ToXElementPersonVisitor : PersonVisitor<XElement>
        {
            protected override XElement Visit(Person p)
            {
                return new XElement("Person",
                    new XAttribute("Type", p.GetType().Name),
                    new XElement("FirstName", p.FirstName),
                    new XElement("LastName", p.LastName),
                    p.Friends.Select(f => DynamicVisit(f))
                );
            }

            protected override XElement Visit(Customer c)
            {
                XElement xe = base.Visit(c);
                xe.Add(new XElement("CreditLimit", c.CreditLimit));
                return xe;
            }

            protected override XElement Visit(Employee e)
            {
                XElement xe = base.Visit(e);
                xe.Add(new XElement("Salary", e.Salary));
                return xe;
            }
        }


        class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            // The Friends collection may contain Customers & Employees:
            public readonly IList<Person> Friends = new Collection<Person>();
        }

        class Customer : Person { public decimal CreditLimit { get; set; } }
        class Employee : Person { public decimal Salary { get; set; } }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.testType
{
    class Tuple
    {
        public static void Test()
        {
            {
                var bob = ("Bob", 23, "mike"); // Allow compiler to infer the element types

                Console.WriteLine(bob.Item1); // Bob
                Console.WriteLine(bob.Item2); // 23
                Console.WriteLine(bob.Item3); // 23

                // Tuples are mutable value types:

                var joe = bob; // joe is a *copy* of job
                joe.Item1 = "Joe"; // Change joe’s Item1 from Bob to Joe
                Console.WriteLine(bob); // (Bob, 23)
                Console.WriteLine(joe); // (Joe, 23)

                (string, int) Mike = ("mike", 23); // var is not compulsory with tuples!
                //(var, var) Peter = ("mike", 23);   // var is not compulsory with tuples!

            }

            {
                (string, int) person = GetPerson(); // Could use 'var' here if we want
                var personTwo = GetPerson(); // Could use 'var' here if we want
                Console.WriteLine(person.Item1); // Bob
                Console.WriteLine(person.Item2); // 23
                // here the static is to prevent this method reference  variables within the Enclosing Method
                string name = "BOB";
                static (string, int) GetPerson() => ("BOB", 23);
                (string, int) GetPersonTWO() => (name, 23); // PAY ATTENTION TO the static
            }
            {
                //naming Tuple
                var tuple = (Name: "Bob", Age: 23);

                Console.WriteLine(tuple.Name); // Bob
                Console.WriteLine(tuple.Age); // 23


                var person = GetPerson();
                Console.WriteLine(person.Name); // Bob
                Console.WriteLine(person.Age); // 23

                static (string Name, int Age) GetPerson() => ("Bob", 23);
            }
            {
                // Tuple Create with Factory Method

                ValueTuple<string, int> bob1 = ValueTuple.Create("Bob", 23);
                (string, int) bob2 = ValueTuple.Create("Bob", 23);
                // Can define more than 2 elements
                var numbers = (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
                (int, string, string) personOne = (1, "Bill", "Gates");

                // this is Tuple Class, not ValueTuple
                Tuple<int, string, string> person =
                    new Tuple<int, string, string>(1, "Steve", "Jobs");

            }
            {
                //Decontruct Tuple
                var bob = ("Bob", 23);

                (string name, int age) = bob; // Deconstruct the bob tuple into
                // separate variables (name and age).
                Console.WriteLine(name);
                Console.WriteLine(age);
            }
            {
                var (name, age, sex) = GetBobTwo();
                Console.WriteLine(name); // Bob
                Console.WriteLine(age); // 23
                Console.WriteLine(sex); // M

                static (string, int, char) GetBobTwo() => ("Bob", 23, 'M');
            }
            // Comparison
            {
                var t1 = ("one", 1);
                var t2 = ("one", 1);
                Console.WriteLine(t1.Equals(t2)); // True
            }

            {
                // Linq operation
                var tuples = new[]
                {
                    ("B", 50),
                    ("B", 40),
                    ("A", 30),
                    ("A", 20)
                };

                var a = tuples.OrderBy(x => x);
                (string, int) b = a.ElementAt(0);



            }
        }
    }
}

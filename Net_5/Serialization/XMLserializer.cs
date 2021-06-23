using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Net_5.Serialization
{
    class XMLserializer
    {
        public static void Test()
        {
            #region Attribute-based serialization


            //Attribute-based serialization---Start
            {
                //Person p = new Person();
                //p.Name = "Stacey"; p.Age = 30;

                //var xs = new XmlSerializer(typeof(Person));// 要告诉它TYPE

                //using (Stream s = File.Create("person.xml"))   // 做一个STREAM
                //    xs.Serialize(s, p);                            // 注意构造方法，把STREAM 和具体的OJBECT放进去


                ////DESERIALIZE
                //Person p2;
                //using (Stream s = File.OpenRead("person.xml"))
                //    p2 = (Person)xs.Deserialize(s);

                //Console.WriteLine(p2.Name + " " + p2.Age);   // Stacey 30

                //File.ReadAllText("person.xml").Dump("XML");
            }

            //Attribute - based serialization---Attribute Name and namespace
            
            {
                //one.Person p = new one.Person();
                //p.Name = "Stacey"; p.Age = 30;

                //var xs = new XmlSerializer(typeof(one.Person));

                //using (Stream s = File.Create("person.xml"))
                //    xs.Serialize(s, p);


                //one.Person p2;
                //using (Stream s = File.OpenRead("person.xml"))
                //    p2 = (one.Person)xs.Deserialize(s);

                //Console.WriteLine(p2.Name + " " + p2.Age);   // Stacey 30

                //File.ReadAllText("person.xml").Dump("XML");
            }

            //Attribute - based serialization---XML Elements Order
            {
                //two.Person p = new two.Person();
                //p.Name = "Stacey"; p.Age = 30;

                //var xs = new XmlSerializer(typeof(two.Person));

                //using (Stream s = File.Create("person.xml"))
                //    xs.Serialize(s, p);

                //two.Person p2;
                //using (Stream s = File.OpenRead("person.xml"))
                //    p2 = (two.Person)xs.Deserialize(s);

                //Console.WriteLine(p2.Name + " " + p2.Age);   // Stacey 30

                //File.ReadAllText("person.xml").Dump("XML");
            }
            #endregion

            #region Subclass and Child Objects

            {
                // Subclassing root type
                // TODO: 有两种方法，这里仅是第一种
                {
                    //var p = new three.Student { Name = "Stacey" };
                    //SerializePerson(p, "person.xml");
                    //File.ReadAllText("person.xml").Dump("XML");
                    ////从XML转为OBJECT，
                    //three.Person p2 =DeSerializePerson("person.xml");
                    //three.Student p3 = DeSerializeStudent("person.xml");
                    //// Person 是父CLASS, 
                    ///*
                    // <?xml version="1.0"?>
                    //    <Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xsi:type="Student">
                    //      <Name>Stacey</Name>
                    //    </Person>
                    // */
                }

                //  Serialize Child Objects
                {
                    //four.Person p = new four.Person { Name = "Stacey" };
                    //p.HomeAddress.Street = "Odo St";
                    //p.HomeAddress.PostCode = "6020";

                    //SerializePerson(p, "person.xml");
                    //File.ReadAllText("person.xml").Dump("XML");

                    ///*
                    // <?xml version="1.0"?>
                    //    <Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                    //      <Name>Stacey</Name>
                    //      <HomeAddress>
                    //        <Street>Odo St</Street>
                    //        <PostCode>6020</PostCode>
                    //      </HomeAddress>
                    //    </Person>
                    // */
                }

                //  Serialize Child Objects---Option 1 注意子类用ATTRIBUTE来标志
                {
                    //five.Person p = new five.Person { Name = "Stacey" };
                    //p.HomeAddress.Street = "Odo St";
                    //p.HomeAddress.PostCode = "6020";

                    //SerializePerson(p, "person.xml");
                    //File.ReadAllText("person.xml").Dump("XML");
                    ///*
                    // <?xml version="1.0"?>
                    //    <Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                    //      <Name>Stacey</Name>
                    //      <HomeAddress xsi:type="USAddress">
                    //        <Street>Odo St</Street>
                    //        <PostCode>6020</PostCode>
                    //      </HomeAddress>
                    //    </Person>
                    // */
                }
                //
                {
                    //six.Person p = new six.Person { Name = "Stacey" };
                    //p.HomeAddress.Street = "Odo St";
                    //p.HomeAddress.PostCode = "6020";

                    //SerializePerson(p, "person.xml");
                    //File.ReadAllText("person.xml").Dump("XML");

                }
            }

            #endregion


        }
        public static void SerializePerson(three.Person p, string path)
        {
            // XmlSerializer 是用父类初始化的，要处理的内容却是子类 STUDENT
            // TODO: 这里用父类来实始化这个XMLSERIALIZER，是为了更通用？？？？
            XmlSerializer xs = new XmlSerializer(typeof(three.Person)); 
            using (Stream s = File.Create(path))
                xs.Serialize(s, p);
        }

        public static three.Person DeSerializePerson(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(three.Person)); // XmlSerializer 是用父类初始化的，要处理的内容却是子类 STUDENT
            using (Stream s = File.OpenRead(path))
                return (three.Person)xs.Deserialize(s);
        }

        public static three.Student DeSerializeStudent(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(three.Person)); // XmlSerializer 是用父类初始化的，要处理的内容却是子类 STUDENT
            using (Stream s = File.OpenRead(path))
                return (three.Student)xs.Deserialize(s);
        }

        public static void SerializePerson(four.Person p, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(four.Person));
            using (Stream s = File.Create(path))
                xs.Serialize(s, p);
        }

        public static void SerializePerson(five.Person p, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(five.Person));
            using (Stream s = File.Create(path))
                xs.Serialize(s, p);
        }

        public static void SerializePerson(six.Person p, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(six.Person));
            using (Stream s = File.Create(path))
                xs.Serialize(s, p);
        }
    }

    public class Person
    {
        public string Name;
        public int Age;
    }
    /*
    <?xml version="1.0"?>
    <Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <Name>Stacey</Name>
      <Age>30</Age>
    </Person>
 */



    namespace one
    {
        public class Person
        {
            [XmlElement("FirstName")] public string Name;
            [XmlAttribute("RoughAge")] public int Age;      // 标记为XMLAttribute，则写在根结点中
        }
        /*
    <?xml version="1.0"?>
    <Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" RoughAge="30">
      <FirstName>Stacey</FirstName>
    </Person>
 */
    }

    namespace two
    {
        public class Person
        {
            [XmlElement(Order = 2)] public string Name;    // 
            [XmlElement(Order = 1)] public int Age;
        }

        /*
 <?xml version="1.0"?>
    <Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <Age>30</Age>
      <Name>Stacey</Name>
    </Person>
 */
    }

    namespace three
    {
        [XmlInclude(typeof(Student))] //告诉父类自己的存在
        [XmlInclude(typeof(Teacher))]
        public class Person { public string Name; }

        public class Student : Person { }
        public class Teacher : Person { }

        /*
            <?xml version="1.0"?>
            <Person xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xsi:type="Student">
              <Name>Stacey</Name>
            </Person>
         */
    }

    namespace four
    {
        public class Person
        {
            public string Name;
            public Address HomeAddress = new Address();
        }

        public class Address { public string Street, PostCode; }
    }

    namespace five
    {
        [XmlInclude(typeof(AUAddress))]
        [XmlInclude(typeof(USAddress))]
        public class Address { public string Street, PostCode; }

        public class USAddress : Address { }
        public class AUAddress : Address { }

        public class Person
        {
            public string Name;
            public Address HomeAddress = new USAddress();
        }
    }

    namespace six
    {
        public class Address { public string Street, PostCode; }

        public class USAddress : Address { }
        public class AUAddress : Address { }

        public class Person
        {
            public string Name;

            [XmlElement("Address", typeof(Address))]
            [XmlElement("AUAddress", typeof(AUAddress))]
            [XmlElement("USAddress", typeof(USAddress))]
            public Address HomeAddress = new USAddress();
        }
    }
}

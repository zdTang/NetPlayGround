using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Collection
{
    class ListSoOn
    {
        public static void Test()
        {
            #region ArrayList List

            {
                // ArrayList, List内部都是ARRAY,因而有INDEX, 速度快
                //===========ArrayList

                ArrayList al = new ArrayList();
                al.Add("hello");
                string first = (string)al[0];
                string[] strArr = (string[])al.ToArray(typeof(string));  //需要类型转换

                ArrayList all = new ArrayList();
                all.AddRange(new[] { 1, 5, 9 });
                List<int> list = all.Cast<int>().ToList();

                //===============List<T>

                List<string> words = new List<string>();    // New string-typed list

                words.Add("melon");
                words.Add("avocado");
                words.AddRange(new[] { "banana", "plum" });
                words.Insert(0, "lemon");                           // Insert at start
                words.InsertRange(0, new[] { "peach", "nashi" });   // Insert at start

                words.Remove("melon");
                words.RemoveAt(3);                         // Remove the 4th element
                words.RemoveRange(0, 2);                   // Remove first 2 elements

                // Remove all strings starting in 'n':
                words.RemoveAll(s => s.StartsWith("n"));

                Console.WriteLine(words[0]);                          // first word
                Console.WriteLine(words[words.Count - 1]);            // last word
                foreach (string s in words) Console.WriteLine(s);      // all words
                List<string> subset = words.GetRange(1, 2);            // 2nd->3rd words

                string[] wordsArray = words.ToArray();    // Creates a new typed array

                // Copy first two elements to the end of an existing array:
                string[] existing = new string[1000];
                words.CopyTo(0, existing, 998, 2);

                List<string> upperCaseWords = words.ConvertAll(s => s.ToUpper());
                List<int> lengths = words.ConvertAll(s => s.Length);
            }

            #endregion

            #region LinkedList<T>
            //https://www.ictdemy.com/csharp/collections-and-linq/linked-lists-in-c-net
            // The reason LinkedList doesn't support random access natively is because it's a rather inefficient operation for the data structure.
            // If you're going to be doing it often you should think about using a more appropriate data structure.
            // LINKEDLIST用NODE互连实现，没有INDEX, 虽然可以做，但官方没有提供
            //在需要操作中间元素时，用LINKEDLIST快。因为可以先建一中间节点，然后以之中心，前后加，见LINK
            {
                var tune = new LinkedList<string>();
                tune.AddFirst("do"); tune.Dump("one"); // do
                tune.AddLast("so"); tune.Dump("two"); // do - so

                tune.AddAfter(tune.First, "re"); tune.Dump("three"); // do - re- so
                tune.AddAfter(tune.First.Next, "mi"); tune.Dump("four"); // do - re - mi- so
                tune.AddBefore(tune.Last, "fa"); tune.Dump("five"); // do - re - mi - fa- so

                tune.RemoveFirst(); tune.Dump("removeFirst"); // re - mi - fa - so
                tune.RemoveLast(); tune.Dump("removeLast"); // re - mi - fa

                LinkedListNode<string> miNode = tune.Find("mi");   // 可以找到一个NODE
                tune.Remove(miNode); tune.Dump("remove"); // re - fa
                tune.AddFirst(miNode);
            }

            #endregion

            #region Queue

            {
                var q = new Queue<int>();
                q.Enqueue(10);
                q.Enqueue(20);
                int[] data = q.ToArray();         // Exports to an array
                Console.WriteLine(q.Count);      // "2"
                Console.WriteLine(q.Peek());     // "10"
                Console.WriteLine(q.Dequeue());  // "10"
                Console.WriteLine(q.Dequeue());  // "20"
                //Console.WriteLine(q.Dequeue());  // throws an exception (queue empty)
            }

            #endregion

            #region Stack

            {
                var s = new Stack<int>();
                s.Push(1);                      //            Stack = 1
                s.Push(2);                      //            Stack = 1,2
                s.Push(3);                      //            Stack = 1,2,3
                Console.WriteLine(s.Count);     // Prints 3
                Console.WriteLine(s.Peek());    // Prints 3,  Stack = 1,2,3
                Console.WriteLine(s.Pop());     // Prints 3,  Stack = 1,2
                Console.WriteLine(s.Pop());     // Prints 2,  Stack = 1
                Console.WriteLine(s.Pop());     // Prints 1,  Stack = <empty>
                //Console.WriteLine(s.Pop());     // throws exception
            }

            #endregion

            #region BitArray

            {
                var bits = new BitArray(2);// all default value are FALSE
                bits[1] = true;
                bits.Xor(bits);               // Bitwise exclusive-OR bits with itself//XOR两个不一样时为TRUE,一样时为FALSE
                Console.WriteLine(bits[1]);   // False
            }

            #endregion

            #region HashSet and Sorted Set

            {
                // HASHSET底层由HASH TABLE实现，只存KEY, DICTIONARY还存VALUE
                {
                    // 这种结构用来过滤重复内容很好用
                    var letters = new HashSet<char>("the quick brown fox"); // 用HASH TABLE实现， HASH值上可能有重复，一个BUCKET中可能有重复值

                    Console.WriteLine(letters.Contains('t'));      // true
                    Console.WriteLine(letters.Contains('j'));      // false

                    foreach (char c in letters) Console.Write(c);   // the quickbrownfx
                }
                Console.WriteLine();
                {
                    var letters = new SortedSet<char>("the quick brown fox");//用RED BLACK TREE实现，放入时已经排序了

                    foreach (char c in letters)
                        Console.Write(c);                                    //  bcefhiknoqrtuwx

                    Console.WriteLine();

                    foreach (char c in letters.GetViewBetween('f', 'i'))
                        Console.Write(c);                                    //  fhi
                }
                // 由于是集合SET，它支持一些集合操作
                {
                    var letters = new HashSet<char>("the quick brown fox");
                    letters.IntersectWith("aeiou");
                    foreach (char c in letters) Console.Write(c);     // euio
                }
                Console.WriteLine();
                {
                    var letters = new HashSet<char>("the quick brown fox");
                    letters.ExceptWith("aeiou");
                    foreach (char c in letters) Console.Write(c);     // th qckbrwnfx
                }
                Console.WriteLine();
                {
                    var letters = new HashSet<char>("the quick brown fox");
                    letters.SymmetricExceptWith("the lazy brown fox");
                    foreach (char c in letters) Console.Write(c);     // quicklazy
                }

            }

            #endregion

            #region Dictionary<k,v>

            {
                //DICTIONARY 没有顺序，KEY不可以重复，重复的话VALUE会被UPDATE
                //底层由HASH TABLE实现，其实DICTIONARY就是个HASH TABLE
                //其底层HASH TABLE的实现，见NUTSHELL, HASH CODE->HASH KEY->BUCKET
                var d = new Dictionary<string, int>();
                // ======添加值,可以用索引法，或ADD()======
                d.Add("One", 1);
                //d.Add("One", 2);  // 重复会EXCEPTION,但用INDEX的话，重复则只会覆盖旧值，如下
                d["Two"] = 2;     // adds to dictionary because "two" not already present
                d["Two"] = 22;    // updates dictionary because "two" is now present
                d["Three"] = 3;
                // =====取值用索引法，或TRYGETVALUE()
                Console.WriteLine(d["Two"]);                 // Prints "22"
                Console.WriteLine(d.ContainsKey("One"));     // true (fast operation)
                Console.WriteLine(d.ContainsValue(3));       // true (slow operation)
                int val = 0;
                if (!d.TryGetValue("onE", out val))         
                    Console.WriteLine("No val");            // "No val" (case sensitive)
                //====TRAVERSE有三种形式
                // Three different ways to enumerate the dictionary:
                // 同时取出KEY,VALUE，用 KeyValuePair<string, int> KV 的格式 
                foreach (KeyValuePair<string, int> kv in d)          //  One; 1
                    Console.WriteLine(kv.Key + "; " + kv.Value);    //  Two; 22
                //  Three; 3


                //只取KEY
                foreach (string s in d.Keys) Console.Write(s);      // OneTwoThree
                Console.WriteLine();
                //只取VALUE
                foreach (int i in d.Values) Console.Write(i);       // 1223
                //设置忽略IGNORE KEY的大小写
                var dIgnoreCase = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                dIgnoreCase["foo"] = true;
                dIgnoreCase["FOO"].Dump("foo");
            }


            #endregion

            #region Ordered Dictionary

            {
                //https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.ordereddictionary?view=net-5.0
                // 注意，这是ORDERED, 而不是SORTED,是两回事
                // Creates and initializes a OrderedDictionary.
                OrderedDictionary myOrderedDictionary = new OrderedDictionary();
                myOrderedDictionary.Add("testKey1", "testValue1");
                myOrderedDictionary.Add("testKey2", "testValue2");
                myOrderedDictionary.Add("keyToDelete", "valueToDelete");
                myOrderedDictionary.Add("testKey3", "testValue3");

                ICollection keyCollection = myOrderedDictionary.Keys;
                ICollection valueCollection = myOrderedDictionary.Values;

                // Display the contents using the key and value collections
                DisplayContents(keyCollection, valueCollection, myOrderedDictionary.Count);
                // Modifying the OrderedDictionary
                if (!myOrderedDictionary.IsReadOnly)
                {
                    // Insert a new key to the beginning of the OrderedDictionary
                    myOrderedDictionary.Insert(0, "insertedKey1", "insertedValue1");

                    // Modify the value of the entry with the key "testKey2"
                    myOrderedDictionary["testKey2"] = "modifiedValue";

                    // Remove the last entry from the OrderedDictionary: "testKey3"
                    myOrderedDictionary.RemoveAt(myOrderedDictionary.Count - 1);

                    // Remove the "keyToDelete" entry, if it exists
                    if (myOrderedDictionary.Contains("keyToDelete"))
                    {
                        myOrderedDictionary.Remove("keyToDelete");
                    }
                }

                Console.WriteLine(
                    "{0}Displaying the entries of a modified OrderedDictionary.",
                    Environment.NewLine);
                DisplayContents(keyCollection, valueCollection, myOrderedDictionary.Count);

                // Clear the OrderedDictionary and add new values
                myOrderedDictionary.Clear();
                myOrderedDictionary.Add("newKey1", "newValue1");
                myOrderedDictionary.Add("newKey2", "newValue2");
                myOrderedDictionary.Add("newKey3", "newValue3");

                // Display the contents of the "new" Dictionary using an enumerator
                IDictionaryEnumerator myEnumerator =
                    myOrderedDictionary.GetEnumerator();

                Console.WriteLine(
                    "{0}Displaying the entries of a \"new\" OrderedDictionary.",
                    Environment.NewLine);

                DisplayEnumerator(myEnumerator);

            }

            #endregion

            #region SortedList

            {
                // 虽然叫LIST，也是DICTIONARY的一种，以KEY+VALUE存数据
                // 与SORTED DICTIONARY各有所长，见NETSHELL
                // 底层由ARRAY PAIR实现，而SORTED DICTIONARY底层由RED BLACK TREE实现
                // RETRIVAL很快，但INSERTION慢，因为要SHIFT数据
                // demo拿到OBJECT CLASS的TYPE数据，然后以NAME为KEY,存入SORTEDLIST中
                // MethodInfo is in the System.Reflection namespace

                var sorted = new SortedList<string, MethodInfo>();

                    foreach (MethodInfo m in typeof(object).GetMethods())
                        sorted[m.Name] = m;  //可以以KEY取值

                    sorted.Keys.Dump("keys");
                    sorted.Values.Dump("values");

                    foreach (MethodInfo m in sorted.Values)
                        Console.WriteLine(m.Name + " returns a " + m.ReturnType);

                    Console.WriteLine(sorted["GetHashCode"]);      // Int32 GetHashCode()

                    Console.WriteLine(sorted.Keys[sorted.Count - 1]);            // ToString //可以用INDEX取值
                    Console.WriteLine(sorted.Values[sorted.Count - 1].IsVirtual);  // True

                
            }

            #endregion

            #region SortedDictionary

            {
                // 底层由RED BLACK TREE构成，INSERTION比SORTEDLIST快，但RETRIVAL不如SORTEDLIST快,见nutshell
                // Create a new sorted dictionary of strings, with string
                // keys.
                SortedDictionary<string, string> openWith =
                    new SortedDictionary<string, string>();

                // Add some elements to the dictionary. There are no
                // duplicate keys, but some of the values are duplicates.
                openWith.Add("txt", "notepad.exe");
                openWith.Add("bmp", "paint.exe");
                openWith.Add("dib", "paint.exe");
                openWith.Add("rtf", "wordpad.exe");

                // The Add method throws an exception if the new key is
                // already in the dictionary.
                try
                {
                    openWith.Add("txt", "winword.exe");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("An element with Key = \"txt\" already exists.");
                }

                // The Item property is another name for the indexer, so you
                // can omit its name when accessing elements.
                Console.WriteLine("For key = \"rtf\", value = {0}.",
                    openWith["rtf"]);

                // The indexer can be used to change the value associated
                // with a key.
                openWith["rtf"] = "winword.exe";
                Console.WriteLine("For key = \"rtf\", value = {0}.",
                    openWith["rtf"]);

                // If a key does not exist, setting the indexer for that key
                // adds a new key/value pair.
                openWith["doc"] = "winword.exe";

                // The indexer throws an exception if the requested key is
                // not in the dictionary.
                try
                {
                    Console.WriteLine("For key = \"tif\", value = {0}.",
                        openWith["tif"]);
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Key = \"tif\" is not found.");
                }

                // When a program often has to try keys that turn out not to
                // be in the dictionary, TryGetValue can be a more efficient
                // way to retrieve values.
                string value = "";
                if (openWith.TryGetValue("tif", out value))
                {
                    Console.WriteLine("For key = \"tif\", value = {0}.", value);
                }
                else
                {
                    Console.WriteLine("Key = \"tif\" is not found.");
                }

                // ContainsKey can be used to test keys before inserting
                // them.
                if (!openWith.ContainsKey("ht"))
                {
                    openWith.Add("ht", "hypertrm.exe");
                    Console.WriteLine("Value added for key = \"ht\": {0}",
                        openWith["ht"]);
                }

                // When you use foreach to enumerate dictionary elements,
                // the elements are retrieved as KeyValuePair objects.
                Console.WriteLine();
                foreach (KeyValuePair<string, string> kvp in openWith)
                {
                    Console.WriteLine("Key = {0}, Value = {1}",
                        kvp.Key, kvp.Value);
                }

                // To get the values alone, use the Values property.
                SortedDictionary<string, string>.ValueCollection valueColl =
                    openWith.Values;

                // The elements of the ValueCollection are strongly typed
                // with the type that was specified for dictionary values.
                Console.WriteLine();
                foreach (string s in valueColl)
                {
                    Console.WriteLine("Value = {0}", s);
                }

                // To get the keys alone, use the Keys property.
                SortedDictionary<string, string>.KeyCollection keyColl =
                    openWith.Keys;

                // The elements of the KeyCollection are strongly typed
                // with the type that was specified for dictionary keys.
                Console.WriteLine();
                foreach (string s in keyColl)
                {
                    Console.WriteLine("Key = {0}", s);
                }

                // Use the Remove method to remove a key/value pair.
                Console.WriteLine("\nRemove(\"doc\")");
                openWith.Remove("doc");

                if (!openWith.ContainsKey("doc"))
                {
                    Console.WriteLine("Key \"doc\" is not found.");
                }
            }

            #endregion
        }
        // Displays the contents of the OrderedDictionary from its keys and values
        static void DisplayContents(
            ICollection keyCollection, ICollection valueCollection, int dictionarySize)
        {
            String[] myKeys = new String[dictionarySize];
            String[] myValues = new String[dictionarySize];
            keyCollection.CopyTo(myKeys, 0);
            valueCollection.CopyTo(myValues, 0);

            // Displays the contents of the OrderedDictionary
            Console.WriteLine("   INDEX KEY                       VALUE");
            for (int i = 0; i < dictionarySize; i++)
            {
                Console.WriteLine("   {0,-5} {1,-25} {2}",
                    i, myKeys[i], myValues[i]);
            }
            Console.WriteLine();
        }

        // Displays the contents of the OrderedDictionary using its enumerator
        public static void DisplayEnumerator(IDictionaryEnumerator myEnumerator)
        {
            Console.WriteLine("   KEY                       VALUE");
            while (myEnumerator.MoveNext())
            {
                Console.WriteLine("   {0,-25} {1}",
                    myEnumerator.Key, myEnumerator.Value);
            }
        }
    }
}

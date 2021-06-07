using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Net_5.Linq
{
    class TestLinq
    {
        public static void Test()
        {
            #region Simple Filter

            {
                //利用ENUMERABLE，这是个STATIC CLASS, QUERY local sequence
                //利用Queryable， 这是个static CLASS, QUERY Dynamically Feed Data : DATABASE
                int[] v1 = { 1, 2, 3 }; //First vector
                int[] v2 = { 3, 2, 1 }; //Second vector
                //dot product of vector
                var a = v1.Zip(v2, (a, b) => a + b);

                var b = Enumerable.Range(2, 10)
                    .Select(c => new { Length = 2 * c, Height = c * c - 1, Hypotenuse = c * c + 1 });
            }
            // 最简单的一种形式，只有一个WHERE OPERATOR
            // 很象QUERY:  SELECT * FROM names Where
            {
                string[] names = { "Tom", "Dick", "Harry" };

                
                //可以直接用STATIC ENUMERABLE上的方法
                IEnumerable<string> filteredNames =
                    Enumerable.Where(names, n => n.Length >= 4);
                // foreach 要指定TYPE
                foreach (string n in filteredNames)
                    Console.Write(n + "|");            // Dick|Harry|


                //也可以用继承了IENUMERABLE的集合自带的方法
                //这里用FLUENT SYNTAX
                // "Where" is an extension method in System.Linq.Enumerable:
                IEnumerable<string> filteredNamesTwo =
                    names.Where(n => n.Length >= 4);
                

                // In LINQPad, we can also write query results using Dump:

                filteredNamesTwo.Dump("Simple use of 'Where' query operator");


                //LINQ形式之二Query Expression   // 这个N是SEQUENCE中的一个ELEMENT
                //这种形式与FLUENT SYNTAX是互补的
                IEnumerable<string> x =from n in new[] {"Tom", "Dick", "Harry"}
                    where n.Contains("a")
                    select n;
            }

            {
                int[] values = { 1, 2, 3 };
                int[] weights = { 3, 2, 1 };
                //dot product of vector
                var a = values.Zip(weights, (value, weight) => value * weight); //same as a dot product;
                var b = values.Zip(weights, (value, weight) => value * weight).Sum();//same as a dot product

            }
            {
                int[] nums = { 20, 15, 31, 34, 35, 40, 50, 90, 99, 100 };
                var a = nums
                    .ToLookup(k => k, k => nums.Where(n => n < k))
                    .Select(k => new KeyValuePair<int, double>
                        (k.Key, 100 * ((double)k.First().Count() / (double)nums.Length)));
                var b = nums
                    .ToLookup(k => k, k => nums.Where(n => n < k));

            }

            #endregion

            #region Fluent Syntax

                #region Chaining Query
            {
                //projector, generator,filter, statistical四种类型的FUNCTION
                string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };


                IEnumerable<string> query = names
                    .Where(n => n.Contains("a"))    //filter
                    .OrderBy(n => n.Length)         //generator
                    .Select(n => n.ToUpper());      // WHERE 是挑出来， 而SELECT是全部都执行某种操作？Projector

                query.Dump();

                // The same query constructed progressively:

                IEnumerable<string> filtered = names.Where(n => n.Contains("a"));
                // 注意ORDERBY中两个参数，一个是DATA,另一个是FUNC, 这个FUNC接一个值，返一个值
                // 用返回的这个值来SORT,这里返回的是LENGTH
                IEnumerable<string> sorted = filtered.OrderBy(n => n.Length);
                IEnumerable<string> finalQuery = sorted.Select(n => n.ToUpper());

                filtered.Dump("Filtered");
                sorted.Dump("Sorted");
                finalQuery.Dump("FinalQuery");
            }


            #endregion

                #region Shunning Extension 回避的意思。 EXTENSION是非常有用的，可以写的很流畅，不NESTED,易读

            {
                string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

                IEnumerable<string> query =
                    Enumerable.Select(
                        Enumerable.OrderBy(
                            Enumerable.Where(
                                names, n => n.Contains("a")
                            ), n => n.Length
                        ), n => n.ToUpper()
                    );

                query.Dump("The correct result, but an untidy query!");
            }

            #endregion

                #region Type Inference, Natural Ordering,

            {
                //Type Inference

                string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };
                // 注意这个SELECT的用法，并不是挑选，而是全部元素都PROJECT
                names.Select(n => n.Length).Dump("Notice result is IEnumerable<Int32>; Int32 is inferred");

                IEnumerable<string> sortedByLength, sortedAlphabetically;

                names.OrderBy(n => n.Length).Dump("Integer sorting key");
                names.OrderBy(n => n).Dump("String sorting key");

                // Natural Ordering,

                int[] numbers = { 10, 9, 8, 7, 6 };

                // The natural ordering of numbers is honored, making the following queries possible:

                numbers.Take(3).Dump("Take(3) returns the first three numbers in the sequence");
                numbers.Skip(3).Dump("Skip(3) returns all but the first three numbers in the sequence");
                numbers.Reverse().Dump("Reverse does exactly as it says");
            }

            #endregion

                #region Other Operators

            {
                int[] numbers = { 10, 9, 8, 7, 6 };

                "".Dump("All of these operators are covered in more detail in Chapter 9.");

                // Element operators:

                numbers.First().Dump("First");
                numbers.Last().Dump("Last");

                numbers.ElementAt(1).Dump("Second number");
                numbers.OrderBy(n => n).First().Dump("Lowest number");
                numbers.OrderBy(n => n).Skip(1).First().Dump("Second lowest number");

                // Aggregation operators:

                numbers.Count().Dump("Count");
                numbers.Min().Dump("Min");

                // Quantifiers:

                numbers.Contains(9).Dump("Contains (9)");
                numbers.Any().Dump("Any");  // ANY是有没有的意思
                numbers.Any(n => n % 2 != 0).Dump("Has an odd numbered element");

                // Set based operators:

                int[] seq1 = { 1, 2, 3 };
                int[] seq2 = { 3, 4, 5 };
                seq1.Concat(seq2).Dump("Concat"); // HAVE DUPLICATION
                seq1.Union(seq2).Dump("Union");//NO DUPLICATION
            }

            #endregion


            #endregion

            #region Query Expression

            {
                string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

                IEnumerable<string> query =
                    from n in names
                    where n.Contains("a")   // Filter elements
                    orderby n.Length           // Sort elements
                    select n.ToUpper();       // Translate each element (project)

                query.Dump();
            }
            // Numerable 可以转为 Queryable
            {
                // With AsQueryable() added, you can see the translation to fluent syntax in the λ tab below:

                var names = new[] { "Tom", "Dick", "Harry", "Mary", "Jay" }.AsQueryable();
                // QUERYABLE是另一种形式，不是LOCAL的
                IEnumerable<string> query =
                    from n in names
                    where n.Contains("a")    // Filter elements
                    orderby n.Length            // Sort elements
                    select n.ToUpper();        // Translate each element (project)

                query.Dump();
            }

            // mixing SYNTAX
            {
                string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

                (from n in names where n.Contains("a") select n).Count()
                    .Dump("Names containing the letter 'a'");

                //string first = (from n in names orderby n select n).First()
                   // .Dump("First name, alphabetically");

                names.Where(n => n.Contains("a")).Count()
                    .Dump("Original query, entirely in fluent syntax");

                names.OrderBy(n => n).First()
                    .Dump("Second query, entirely in fluent syntax");
            }


            #endregion

            #region Deferred Execution
            //Introduction
            {
                var numbers = new List<int>();
                numbers.Add(1);
                //QUERY与COLLECTION之间是个引用关系，COLLECTION变，则QUERY的结果也变，数据源始终指向那一个
                IEnumerable<int> query = numbers.Select(n => n * 10);    // Build query

                numbers.Add(2);                    // Sneak in an extra element
                // 构建好的QUERY只有在启动时，才运行，此时已经有2了
                query.Dump("Notice both elements are returned in the result set");
            }
            //reevaluation
            {
                //每次用，都要重新看看值，这就产生两点不好
                //一是我们需要FREEZE当前的状态
                //二是计算很复杂，数据来自REMOTE DB，因而REEVALUATION很OVERHEAD,开销很大
                var numbers = new List<int>() { 1, 2 };

                IEnumerable<int> query = numbers.Select(n => n * 10);

                query.Dump("Both elements are returned");

                numbers.Clear();
                //要非常小心这里，需要FREEZE住 QUERY的值
                query.Dump("All the elements are now gone!");
            }
            //defeat reevaluation
            {
                var numbers = new List<int>() { 1, 2 };
                //用TOLIST 锁住当前状态
                List<int> timesTen = numbers
                    .Select(n => n * 10)
                    .ToList();                      // Executes immediately into a List<int>

                numbers.Clear();
                timesTen.Count.Dump("Still two elements present");
            }
            // CAPTURE Variables
            {
                int[] numbers = { 1, 2 };

                int factor = 10;
                IEnumerable<int> query = numbers.Select(n => n * factor);

                factor = 20;

                query.Dump("Notice both numbers are multiplied by 20, not 10");
            }
            // Capture variable in For LOOP
            {
                // Suppose we want to build up a query that strips all the vowels from a string.
                // The following (although inefficient) gives the correct result:

                IEnumerable<char> query = "Not what you might expect";

                query = query.Where(c => c != 'a');
                query = query.Where(c => c != 'e');
                query = query.Where(c => c != 'i');
                query = query.Where(c => c != 'o');
                query = query.Where(c => c != 'u');

                new string(query.ToArray()).Dump("All vowels are stripped, as you'd expect.");

                "Now, let's refactor this. First, with a for-loop:".Dump();

                string vowels = "aeiou";
                //这个FOR LOOP会构建出5个 CLOSURE,每个包含一个QUERY语句，每句都引用vowels[i]
                //由于DEFER或LAZY，都不会执行，因为此时这些QUERY没有被CALL
                //等跑出LOOP时，I已经是5了，它已经超过了“AEIOU"的范围
                //等到下面的FOREACH执行QUERY时，每个QUERY所在的CLOSURE所CAPTURE的I都是5
                
                /*
                for (int i = 0; i < vowels.Length; i++)
                    query = query.Where(c => c != vowels[i]);   // IndexOutOfRangeException

                foreach (char c in query) Console.Write(c);
                */
                
                // An IndexOutOfRangeException is thrown! This is because, as we saw in Chapter 4 
                // (see "Capturing Outer Variables"), the compiler scopes the iteration variable 
                // in the for loop as if it was declared outside the loop. Hence each closure captures
                // the same variable (i) whose value is 5 when the query is enumerated.
            }
            //SOLUTION解决LOOP VARIABLE的方案，在DELEGATE时也会出现这个问题，主要原因也有DELAY执行的关系
            {
                // We can make the preceding query work correctly by assigning the loop variable to another
                // variable declared inside the statement block:

                IEnumerable<char> query = "Not what you might expect";
                string vowels = "aeiou";

                for (int i = 0; i < vowels.Length; i++)
                {
                    char vowel = vowels[i]; //把这个LOOP VARIABLE存在CLOSURE就好
                    query = query.Where(c => c != vowel);//即在QUERY中不要绑定LOOP VARIABLE
                }

                foreach (char c in query) Console.Write(c);
            }
            //下例也存在同样情况， ACTION构建时，并没有执行，等INVOKE时，LOOP VARIABLE已经变化了
            {
                // The solution, if we want to write 012, is to assign the iteration variable to a local
                // variable that’s scoped inside the loop:

                Action[] actions = new Action[3];

                for (int i = 0; i < 3; i++)
                {
                    int loopScopedi = i;
                    actions[i] = () => Console.Write(loopScopedi);
                }

                foreach (Action a in actions) a();     // 012
            }
            // Captureed Variables and foreach
            {
                // Let's now see what happens when you capture the iteration variable of a foreach loop:

                IEnumerable<char> query = "Not what you might expect";
                string vowels = "aeiou";

                foreach (char vowel in vowels)
                    query = query.Where(c => c != vowel);

                foreach (char c in query) Console.Write(c);

                // The output depends on which version of C# you're running! In C# 4.0 and C# 3.0, we
                // get the same problem we had with the for-loop: each loop iteration captures the same
                // variable, whose final value is 'u'. Hence only the 'u' is stripped. The workaround
                // for this is to use a temporary variable (see next example).

                // From C# 5.0, they fixed the compiler so that the iteration variable of a foreach loop
                // is treated as *local* to each loop iteration. Hence our example strips all vowels
                // as expected.
            }
            // Query operator == DECORATOR[似类WHERE,SELECT等装饰器的写法]
            {
                /*
            {
                static IEnumerable<TResult> MySelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
                {
                    foreach (TSource element in source) yield return selector(element);
                }
            }
             */
            }



            #endregion

            #region Subquery
            //basic subquery
            {
                string[] musos = { "Roger Waters", "David Gilmour", "Rick Wright", "Nick Mason" };
                //拿到每一个M（全名）, 按空格劈开，形成一个由单词组成的ARRAY，然后返回最后一个单词，用来排序
                musos.OrderBy(m => m.Split().Last()).Dump("Sorted by last name");
               
            }
            //Reformulation the subquery
            {
                // For more information on subqueries, see Chapter 9, "Projecting"
                // subQuery在内层，为外层QUERY提供数据
                // 下面几例WHERE需要一个LENGTH,由后面的子QUERY提供
                // 而子QUERY又经过一轮运算，SORT后，取最后一个ELEMENT的LENGTH返回
                // 有些写法不好读

                string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

                names.Where(n => n.Length == names.OrderBy(n2 => n2.Length)
                        .Select(n2 => n2.Length).First())
                    .Dump();

                var query =
                    from n in names
                    where n.Length == (from n2 in names orderby n2.Length select n2.Length).First()
                    select n;

                query.Dump("Same thing as a query expression");

                query =
                    from n in names
                    where n.Length == names.OrderBy(n2 => n2.Length).First().Length
                    select n;

                query.Dump("Reformulated");

                query =
                    from n in names
                    where n.Length == names.Min(n2 => n2.Length)
                    select n;

                query.Dump("Same result, using Min aggregation");

            }
            //Avoiding Subquery
            //这种最好读，易读，易维护
            {
                string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

                int shortest = names.Min(n => n.Length);
                (
                        from n in names
                        where n.Length == shortest
                        select n
                    )
                    .Dump("No subquery");

            }
            #endregion

            #region Composition Strategy
            // 这种策略有三种操作方法
            // one: Progressive Query Building
            {
                var names = new[] { "Tom", "Dick", "Harry", "Mary", "Jay" }.AsQueryable();

                (
                    names
                            .Select(n => n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", ""))
                            .Where(n => n.Length > 2)
                            .OrderBy(n => n)
                    )
                    .Dump("This query was written in fluent syntax");

                (
                        from n in names
                        where n.Length > 2
                        orderby n
                        select n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                    )
                    .Dump("An incorrect translation to query syntax");

                IEnumerable<string> query =
                    from n in names
                    select n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "");

                query = from n in query where n.Length > 2 orderby n select n;

                query.Dump("A correct translation to query syntax, querying in two steps");
            }
            // two: Using INTO keywords
            // 用在SELECT, GROUP子句之后
            // 相当于在一个新的结果集上开始新的QUERY,让思路更清晰
            {
                //Queryable  vs Enumerable两者的不同
                /*
                    1.IEnumerable exists in System.Collections Namespace.     [Namespace]
                    2.IQueryable exists in System. Linq Namespace.            [Namespace]
                    3.Both IEnumerable and IQueryable are forward collection.
                    4.IEnumerable doesn’t support lazy loading            【LAZY LOADING】
                    5.IQueryable support lazy loading                     【LAZY LOADING】
                    6.Querying data from a database, IEnumerable execute a select query on the server side, load data in-memory on a client-side and then filter data.???不明白
                    7.Querying data from a database, IQueryable execute the select query on the server side with all filters.
                    8.IEnumerable Extension methods take functional objects.
                    9.IQueryable Extension methods take expression objects means expression tree.
                 *
                 *
                 */


                //IQueryable support lazy loading                     【LAZY LOADING】
                var names = new[] { "Tom", "Dick", "Harry", "Mary", "Jay" }.AsQueryable();

                (
                        from n in names
                        select n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                        into noVowel //SELECT一次后，找个地方先放一放SELECT的结果
                        where noVowel.Length > 2
                        orderby noVowel
                        select noVowel
                    )
                    .Dump("The preceding query revisited, with the 'into' keyword");
            }
            //two: Using INTO keywords : SCOPING RULES[INTO之后，SCOPE发生变化]
            //注意下例中N1,N2的SCOPE
            // INTO之后，就关上了门，开始新的征途
            {
                string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };

                // The following will not compile - "The name 'n1' does not exist in the current context" (try it).
                /*
                var query =
                    from n1 in names
                    select n1.ToUpper()
                    into n2                          // Only n2 is visible from here on.
                    where n1.Contains("x")           // Illegal: n1 is not in scope.
                    select n2;
                */
                // The equivalent in fluent syntax (you wouldn't expect this to compile!):
                
                /*
                var query = names
                    .Select(n1 => n1.ToUpper())
                    .Where(n2 => n1.Contains("x"));     // Error: n1 no longer in scope	
                */
            }
            //Three, Wrapping Queries
            {
                //IQueryable support lazy loading                     【LAZY LOADING】
                var names = new[] { "Tom", "Dick", "Harry", "Mary", "Jay" }.AsQueryable();
                // 注意，第一个IN后面的（ ）将内容包起来
                IEnumerable<string> query =
                    from n1 in
                    (
                        from n2 in names
                        select n2.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                    )
                    where n1.Length > 2
                    orderby n1
                    select n1;

                query.Dump("Here, one query wraps another");

                var sameQuery = names
                    .Select(n => n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", ""))
                    .Where(n => n.Length > 2)
                    .OrderBy(n => n);

                sameQuery.Dump("In fluent syntax, such queries translate to a linear chain of query operators");
            }
            #endregion

            #region Projection Strategy
            //Object initializer
            //SELECT后面接NEW进行实例化一个有名CLASS
            {
                var names = new[] { "Tom", "Dick", "Harry", "Mary", "Jay" }.AsQueryable();

                IEnumerable<TempProjectionItem> temp =
                    from n in names
                    select new TempProjectionItem  //注意这一句到｛ ｝结束，这是CLASS实例,这一句是在构建一个OBJECT,进行INITIALIZE
                    {
                        Original = n,
                        Vowelless = n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                    };

                temp.Dump();
            }

            // Anonymous Types 
            // SELECT之后用NEW实例无名的CLASS
            {
                var names = new[] { "Tom", "Dick", "Harry", "Mary", "Jay" }.AsQueryable();

                var intermediate = from n in names
                    select new
                    {
                        Original = n,
                        Vowelless = n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                    };

                (
                        from item in intermediate
                        where item.Vowelless.Length > 2
                        select item.Original
                    )
                    .Dump();

                // With the into keyword we can do this in one step:

                (
                        from n in names
                        select new
                        {
                            Original = n,
                            Vowelless = n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                        }
                        into temp
                        where temp.Vowelless.Length > 2
                        select temp.Original
                    )
                    .Dump("With the 'into' keyword");
            }

            //LET KEYWORD
            //LET有很多好处，
            {
                var names = new[] { "Tom", "Dick", "Harry", "Mary", "Jay" }.AsQueryable();
                (
                        from n in names
                        let vowelless = n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                        where vowelless.Length > 2
                        orderby vowelless
                        select n               // Thanks to let, n is still in scope.
                    )
                    .Dump();
            }

            #endregion

            #region Interpreted Queries
            using var dbContext = new NutshellContext();  //建一个DBCONTEXT
            // EFcore Query
            {
                

                //现在看这个QUREY,就清晰一些了
                //与ENUMERABLE不同，Enumerable只是QUERY LOCAL COLLECTION. 在RUNTIME时,将QUERY OPERATOR 解析为一系列DECORATOR SEQUENCE
                //QUERYABLE则被SOLVED成EXPRESSION TREE,直接QUERY 远程DB
                
                IQueryable<string> query = from c in dbContext.Customers 
                    where c.Name.Contains("a") 
                    orderby c.Name.Length 
                    select c.Name.ToUpper();

                

                //Fluent Expression
                IQueryable<string> queryTwo = dbContext.Customers.Where(n => n.Name.Contains("a")).OrderBy(n => n.Name.Length)
                    .Select(n => n.Name.ToUpper());

                foreach (string name in query) Console.WriteLine(name);

                foreach (string name in queryTwo) Console.WriteLine(name);

            }
            // Combining Interpreted and Local Queries
            {
                //using var dbContext = new NutshellContext();  //建一个DBCONTEXT
                // This uses a custom 'Pair' extension method, defined below.

                dbContext.Customers
                    .Select(c => c.Name.ToUpper())
                    .Pair()                                 // Local from this point on.
                    .OrderBy(n => n)
                    .Dump();

                // Here's a more substantial example:

                dbContext.Customers
                    .Select(c => c.Name.ToUpper())
                    .OrderBy(n => n)
                    .Pair()                         // Local from this point on.
                    .Select((n, i) => "Pair " + i.ToString() + " = " + n)
                    .Dump();
            }
            //Regex
            {
                Regex wordCounter = new Regex(@"\b(\w|[-'])+\b");

                // The following query throws an exception, because Regex has no equivalent in SQL:

               /*=================================
                var query = dbContext.MedicalArticles
                    .Where(article => article.Topic == "influenza"
                                      && wordCounter.Matches(article.Abstract).Count < 100);

                query.Dump();
               ===================================*/
            }

            #endregion


        }


    }

    class TempProjectionItem
    {
        public string Original;      // Original name
        public string Vowelless;     // Vowel-stripped name
    }
    //这是MODEL，用来MAPPING 一个TABLE中的FIELD
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    
    // We’ll explain the following class in more detail in the next section.
    // DBCONTEXT
    public class NutshellContext : DbContext
    {
        //用UDL文件生成的串是：Provider=SQLOLEDB.1;Password=123456;Persist Security Info=True;User ID=sa;Initial Catalog=nutshell;Data Source=.
        //使用时提示不认Provider,因而将这个FIELD删除，如下就可以了
        //但如何有一种方法能不显示PASSWORD
        private string _connection = "Password=123456;Persist Security Info=True;User ID=sa;Initial Catalog=nutshell;Data Source=.";
        public virtual DbSet<Customer> Customers { get; set; }
        //UseSqlServer等可能不认，需要进一步安装ENTITYFRAME.SQLSERVER这个PACKAGE
        //https://github.com/dotnet/efcore/issues/21361
        protected override void OnConfiguring(DbContextOptionsBuilder builder) => builder.UseSqlServer(_connection);
        //protected override void OnConfiguring(DbContextOptionsBuilder builder) => builder.
        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<Customer>().ToTable("Customer").HasKey(c => c.ID);
    }
    //这是个QUERY OPERATOR
    //由YIELD RETURN 生成，仔细研究一下
    public static class MyExtensions
    {
        public static IEnumerable<string> Pair(this IEnumerable<string> source)
        {
            string firstHalf = null;
            foreach (string element in source)
                if (firstHalf == null)
                    firstHalf = element;
                else
                {
                    yield return firstHalf + ", " + element;
                    firstHalf = null;
                }
        }
    }
}

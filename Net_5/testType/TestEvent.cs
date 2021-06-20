using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.testType
{
    class TestEvent
    {
        public static void Test()
        {
            // There's a standard pattern for writing events. The pattern provides
            // consistency across both Framework and user code.

            Stock stock = new Stock("THPW");
            stock.Price = 27.10M;
            // Register with the PriceChanged event
            stock.PriceChanged += stock_PriceChanged;
            stock.Price = 31.59M;
        }

        //这是外界定义的该事件的一个SUBSCRIBER
        //SUBSCRIBER都采用标准的PATTERN: 用SENDER和EVENTARGS作为参数
        //用EVENTARGS包装参数的好处是，所有的SUBSCRIBER一律统一只接受两个参数，这就规范了BROADCASTER的格式
        //否则，如果参数个数的不同，还要写不同的BROADCASTER + SUBSCRIBERS格式，即EVENT(DELEGATE)格式
        //还要注意SUBSCRIBE的命名
        static void stock_PriceChanged(object sender, PriceChangedEventArgs e)
        {
            if ((e.NewPrice - e.LastPrice) / e.LastPrice > 0.1M)
                Console.WriteLine("Alert, 10% stock price increase!");
        }
        //这里继承EVENTARGS这个CLASS, 用统一格式来打包事件数据
        // 所谓发生EVENT，即程序的STATE发生变化，数据变动

    }

    //传递EVENT参数必须遵守一定的规则
    //要SUBCLASS一个BASE 
    public class PriceChangedEventArgs : EventArgs
    {
        public readonly decimal LastPrice;
        public readonly decimal NewPrice;
        //这个EVENTARGS就是一个某通的CLASS,包装数据用的
        public PriceChangedEventArgs(decimal lastPrice, decimal newPrice)
        {
            LastPrice = lastPrice; NewPrice = newPrice;
        }
    }

    public class Stock
    {
        string symbol;
        decimal price;

        public Stock(string symbol) { this.symbol = symbol; }
        //事件的DELEGATE也规范化了，由于使用EVENTARGS,统一了事件参数的个数和格式，这里就可以用统一的DELEGATE来统一作为EVENT的名字，这是个GENERIC的委托变量， 就是下面的EVENTHANDLER这个
         // 规则一： 无返回值
         // 规则二：接收两个参数：ＯＢＪＥＣＴ和ＥＶＥＮＴＡＲＧＳ
         // 规则三：用ＥＶＥＮＴＨＡＮＤＬＥＲ结尾命名规则

         // 理解：这个EVENT委托就是HANDLER,它是个代理人，可以绑定无数方法，当事件发生后，广播人就会通过代理人，再由代理人找工人来干活儿
         
        public event EventHandler<PriceChangedEventArgs> PriceChanged;

        // CLASS内部要有规则来FIRE这个EVENT, 接受EVENTARGS,然后将THIS, 和E 广播出去
        // 要制定一个ON..方法，它可以理解为事件处理机（尽管它自己并不亲自处理），CLASS要事前做好规划，一旦某事件发生，就按这个ON..方法进行
        // 这个ON..方法就是消化掌握所有的变化情况， 并按照逻辑，决定何时通知DELEGATE来干活
        // 它会先查一个DELEGATE手上有没有人手可用（是不是NULL）,如果不是NULL,就召唤这个EVENT DELEGATE
        // 这个EVENT HANDLER是DELEGATE,理解为代理人或经纪
        // 如一个事件发生，孩子哭了，CLASS内部预定的事件解决器ON..就会通过经纪人，HI, 孩子哭了哈，
        // EVENT DELEGATE经纪上手上一堆联系方式(+=)，它会一一广播通知这些注册的方法，HI,孩子哭了
        // 这些注册的方法接到通知，就会根据自己定义的行为方式活动

        // 几点注意: 
        // 1,PROTECTED VIRTUAL
        // 2, On..作为开头
        // 遇到ON...之类的，都是EVENT应对预案,其基本动作就是查EVENT DELEGATE 是否为NULL,然后让他工作

        // 这里面也可以包含一些逻辑，让它决定何种程序再去叫那些注册的方法

        protected virtual void OnPriceChanged(PriceChangedEventArgs e)
        {
            PriceChanged?.Invoke(this, e);  // 广播，绑定的方法会接受这些信息，从而采取行动
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                if (price == value) return;
                decimal oldPrice = price;
                price = value;
                // 变化了，就通通告诉它，让它决定啥时去通知EVENT DELEGATE
                OnPriceChanged(new PriceChangedEventArgs(oldPrice, price));
            }
        }
    }


}

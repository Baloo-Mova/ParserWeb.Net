using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViKing.Engine;

namespace ParserLite
{
    class BestProxy
    {
        public Queue<Proxy> list = new Queue<Proxy>();
        private string Key { get { return "4fc08fae583472e98d28bf61829cd29e"; } }

        public BestProxy()
        {
            Load();
        }

        public Proxy getProxy()
        {
            return Valid();
        }

        private Proxy Valid()
        {
            while (list.Count > 0)
            {
                try
                {
                    Proxy p = list.Dequeue();
                    if (VkRequest.Request("https://www.google.ru/", proxy: p).ContentUTF8 != String.Empty)
                    {
                        return p;
                    }

                }
                catch
                {

                }
            }


            Load();

            return Valid();
        }

        public void Load()
        {
            var test = VkRequest.Request("http://api.best-proxies.ru/proxylist.txt?key=" + Key + "&limit=0&level=1,2&includeType&google=1&response=400").ContentUTF8;

            foreach (string s in test.Split("\n"))
            {
                string[] temp = s.Split("://");
                if (temp[0] == "http")
                {
                    list.Enqueue(new Proxy(temp[1], ProxyTypes.HTTP));
                }
                else
                    if (temp[0] == "socks4")
                {
                    list.Enqueue(new Proxy(temp[1], ProxyTypes.Socks4));
                }
                else
                        if (temp[0] == "socks5")
                {
                    list.Enqueue(new Proxy(temp[1], ProxyTypes.Socks5));
                }

            }

        }
    }
}

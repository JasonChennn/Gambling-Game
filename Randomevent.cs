using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication6
{
    class Randomevent
    {
        public int times = 0;
        public void start()
        {
            Random r = new Random();
            times += r.Next(0, 3);
            Console.Write(times);
        }
        public int gettimes()
        {
            return times;
        }

        public void resettimes()
        {
            times = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Logic
{
   public class Circle
    {
        public int x { get; set; }
        public int y { get; set; }

        public Circle(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public Circle()
        { }
    }
}

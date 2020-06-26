using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Bouncing_Balls
{
    class Ball
    {
        public Ellipse TheBall { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double XDiff { get; set; }
        public double YDiff { get; set; }
        public double Radius { get; set; }
    }
}

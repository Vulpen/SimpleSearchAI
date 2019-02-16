using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchSpaceAIGame
{
    /// <summary>
    /// Contains the change between two states.
    /// </summary>
    public abstract class ArcBase
    {
        public abstract void printdata();
    }

    public class HanoiArc : ArcBase
    {
        /// <summary>
        /// Constructor that sets source and destination towers.
        /// </summary>
        /// <param name="_s">Source tower</param>
        /// <param name="_d">Destination tower</param>
        public HanoiArc(int _s, int _d)
        {
            sourceTower = _s;
            destTower = _d;
        }
        public override void printdata()
        {
            Console.WriteLine("Move from tower " + sourceTower + " to tower " + destTower);
        }
        public int sourceTower;
        public int destTower;
    }

}

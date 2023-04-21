using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Map
{

    internal class SetList : LinkedList<BinaryNode<Set>>
    {

        public int Dimension;

        public SetList(int dimension)
        {
            Dimension = dimension;
        }
    }
}

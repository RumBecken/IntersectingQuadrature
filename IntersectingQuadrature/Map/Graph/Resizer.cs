using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Map.Decompose
{
    internal static class Resizer
    {

        public static void Resize(BinaryNode<Set> node, Axis direction, double coordinate, Symbol side)
        {
            node.Value.Resize(coordinate, direction, side);

            if (node.Value.HeightDirection == direction)
            {
                if (side == Symbol.Minus)
                {
                    Relocate(node.FirstChild, direction, coordinate);
                }
                else if (side == Symbol.Plus)
                {
                    Relocate(node.SecondChild, direction, coordinate);
                }
            }
            else
            {
                if (node.FirstChild != null)
                {
                    Resize(node.FirstChild, direction, coordinate, side);
                }
                if (node.SecondChild != null)
                {
                    Resize(node.SecondChild, direction, coordinate, side);
                }
            }
        }

        static void Relocate(BinaryNode<Set> node, Axis direction, double coordinate)
        {
            foreach (BinaryNode<Set> ancestor in node.Descendants())
            {
                ancestor.Value.Relocate(coordinate, direction);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Map
{
    internal static class EmbeddedHyperRectangleExtensions {
        public static EmbeddedHyperRectangle Face(this EmbeddedHyperRectangle hR, int direction, Symbol sign)
        {
            if (hR.ActiveDimensions[direction] == false)
            {
                throw new ArgumentException("Height direction not active.");
            }
            EmbeddedHyperRectangle face = hR.Clone();
            --face.Dimension;
            face.Center[direction] = (int)sign * hR.Diameters[direction] / 2.0 + hR.Center[direction];
            face.Diameters[direction] = 0;
            face.ActiveDimensions[direction] = false;

            return face;
        }

        public static void Resize(this EmbeddedHyperRectangle hR, double coordinate, int direction, Symbol side)
        {
            if (hR.ActiveDimensions[direction] == true)
            {
                double a = hR.Center[direction] - (int)side * hR.Diameters[direction] / 2.0;
                double b = coordinate;

                hR.Center[direction] = (a + b) / 2;
                hR.Diameters[direction] = Math.Abs(a - b);
            }
            else
            {
                throw new ArgumentException("Only active dimension can be resized");
            }
        }

        public static void Relocate(this EmbeddedHyperRectangle hR, double coordinate, int direction)
        {
            hR.Center[direction] = coordinate;
        }

        public static EmbeddedHyperRectangle Clone(this EmbeddedHyperRectangle hR)
        {
            EmbeddedHyperRectangle clone = new EmbeddedHyperRectangle(hR.SpaceDimension);
            clone.Dimension = hR.Dimension;
            for (int i = 0; i < clone.SpaceDimension; ++i)
            {
                clone.ActiveDimensions[i] = hR.ActiveDimensions[i];
                clone.Center[i] = hR.Center[i];
                clone.Diameters[i] = hR.Diameters[i];
            }
            return clone;
        }
    }
}

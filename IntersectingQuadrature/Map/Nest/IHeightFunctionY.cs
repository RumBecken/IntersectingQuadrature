using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Map.Nested
{
    interface IHeightFunctionY
    {
        double Y(double x);

        (double Y, double DxY) YdY(double x);

        (double Y, double DxY, double DxxY) YdYddY(double x);

    }
}

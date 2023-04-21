using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map
{
    internal interface IRootFinder
    {
        Tensor1 Root(IScalarFunction phi, Tensor1 a, Tensor1 b);
    }
}

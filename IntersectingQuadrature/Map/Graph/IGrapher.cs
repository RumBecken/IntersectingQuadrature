using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Map.Graph {
    internal interface IGrapher {
        LinkedList<Decomposition> Decompose(IScalarFunction alpha, IHyperRectangle geometry);
    }
}

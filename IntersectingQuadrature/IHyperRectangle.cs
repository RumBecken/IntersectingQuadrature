using System.Collections;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    public interface IHyperRectangle {

        int Dimension { get; }

        Tensor1 Center { get; }

        Tensor1 Diameters { get; }
    }
}

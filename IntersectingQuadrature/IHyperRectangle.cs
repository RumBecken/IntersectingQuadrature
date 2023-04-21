using System.Collections;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    public interface IHyperRectangle {

        int SpaceDimension { get;}

        int BodyDimension { get; }

        Tensor1 Center { get; }

        Tensor1 Diameters { get; }

        BitArray ActiveDimensions { get; }
    }
}

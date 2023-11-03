using System.Collections;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    public interface IEmbeddedHyperRectangle : IHyperRectangle {

        int SpaceDimension { get; set; }

        BitArray ActiveDimensions { get; set; }
    }
}

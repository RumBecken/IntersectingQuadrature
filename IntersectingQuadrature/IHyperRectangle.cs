using System.Collections;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    
    /// <summary>
    /// Generalization of rectangle to arbitrary dimensions.
    /// </summary>
    public interface IHyperRectangle {

        /// <summary>
        /// Dimension of the Hyperrectangle.
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// Geometric center of the Hyperrectangle. A vector of Length <see cref="Dimension"/>.
        /// </summary>
        Tensor1 Center { get; }

        /// <summary>
        /// Diameter of each direction of the hyperrectangle. A vector of Length <see cref="Dimension"/>.
        /// </summary>
        Tensor1 Diameters { get; }
    }
}

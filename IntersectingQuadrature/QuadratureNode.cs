using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {

    /// <summary>
    /// Quadrature point and quadrature weight.
    /// </summary>
    public class QuadratureNode {
        
        /// <summary>
        /// Quadrature Point
        /// </summary>
        public Tensor1 Point;

        /// <summary>
        /// Quadrature Weight
        /// </summary>
        public double Weight;
    }
}

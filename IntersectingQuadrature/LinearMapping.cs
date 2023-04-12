using System;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    internal class LinearMapping : LinearVectorPolynomial, IIntegralMapping {

        double jacobiDeterminant;
        
        public LinearMapping(Tensor2 A) : base(A) {
            jacobiDeterminant = Math.Abs(Algebra.Determinant(A));
        }

        public LinearMapping(Tensor1 A, Tensor2 B) : base(A,B) {
            jacobiDeterminant = Math.Abs(Algebra.Determinant(B));
        }

        public (double J, Tensor1 X) EvaluateAndDeterminant(Tensor1 tilde) {
            Tensor1 x = Evaluate(tilde);
            return ( jacobiDeterminant, x);
        }
    }
}

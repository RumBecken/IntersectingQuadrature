using TensorAnalysis;

namespace IntersectingQuadrature {
    internal class SymmetricLinearMapping : LinearVectorFunction, IIntegralMapping {

        double jacobiDeterminant;
        
        public SymmetricLinearMapping(Tensor2 A) : base(A) {
            jacobiDeterminant = Math.Abs(Algebra.Determinant(A));
        }

        public SymmetricLinearMapping(Tensor1 A, Tensor2 B) : base(A,B) {
            jacobiDeterminant = Math.Abs(Algebra.Determinant(B));
        }

        public (double J, Tensor1 X) EvaluateAndDeterminant(Tensor1 tilde) {
            Tensor1 x = Evaluate(tilde);
            return ( jacobiDeterminant, x);
        }
    }
}

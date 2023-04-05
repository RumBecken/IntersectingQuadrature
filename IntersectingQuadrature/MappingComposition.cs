using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature {
    internal class MappingComposition : VectorComposition, IIntegralMapping {

        IIntegralMapping mapping;

        IIntegralMapping injection;

        public MappingComposition(IIntegralMapping mapping, IIntegralMapping injection) : base(mapping, injection) {
            this.mapping = mapping;
            this.injection = injection;
        }

        public (double J, Tensor1 X) EvaluateAndDeterminant(Tensor1 x) {
            if(M == N){
                return EvaluateAndJacobianDeterminant(x);
            } else{
                return EvaluateAndGramDeterminant(x);
            }
        }

        (double J, Tensor1 X) EvaluateAndJacobianDeterminant(Tensor1 x) {
            (double Ji, Tensor1 tilde) = injection.EvaluateAndDeterminant(x);
            (double Jm, Tensor1 evaluation) = mapping.EvaluateAndDeterminant(tilde);
            double J = Ji * Jm;
            return (J, evaluation);
        }

        public (double J, Tensor1 X) EvaluateAndGramDeterminant(Tensor1 tilde) {
            (Tensor1 f, Tensor2 jacobian) = EvaluateAndJacobian(tilde);
            Tensor2 G = Algebra.Transpose(jacobian) * jacobian;
            double g = Algebra.Determinant(G);
            double J = System.Math.Sqrt(g);
            return (J, f);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorAnalysis;

namespace IntersectingQuadrature {
    internal class Embedding : IIntegralMapping {
        IVectorFunction f;
        
        public Embedding(IVectorFunction f) {
            this.f = f;
        }

        public int M => f.M;

        public int N => f.N;

        public Tensor1 Evaluate(Tensor1 x) {
            return f.Evaluate(x);
        }

        public (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 x) {
            return f.EvaluateAndJacobian(x);
        }

        public (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 x) {
            return f.EvaluateAndJacobianAndHessian(x);
        }

        public (double J, Tensor1 X) EvaluateAndDeterminant(Tensor1 x) {
            return EvaluateAndGramDeterminant(x);
        }

        public (double J, Tensor1 X) EvaluateAndGramDeterminant(Tensor1 tilde) {
            (Tensor1 f, Tensor2 jacobian) = EvaluateAndJacobian(tilde);
            Tensor2 G = Algebra.Transpose(jacobian) * jacobian;
            double g = Algebra.Determinant(G);
            double J = Math.Sqrt(g);
            return (J, f);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using TensorAnalysis;

namespace Example.Experiments {
    internal class Rotation : IVectorFunction {

        LinearVectorFunction f;
        
        public int M => f.M;

        public int N => f.N;

        public Rotation( Tensor1 from, Tensor1 to) {
            Tensor2 d =  Algebra.Dyadic(to, from) - Algebra.Dyadic(from, to);
            Tensor2 Q = d * d;
            Algebra.Scale(Q, 1.0 / (1 + (from * to)));
            Q = Q + d + Tensor2.Unit(from.M);
            f = new LinearVectorFunction(Q);
        }

         public Tensor1 Evaluate(Tensor1 x) {
            return f.Evaluate(x);
        }

        public (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 x) {
            return f.EvaluateAndJacobian(x);
        }

        public (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 x) {
            return f.EvaluateAndJacobianAndHessian(x);
        }
    }
}

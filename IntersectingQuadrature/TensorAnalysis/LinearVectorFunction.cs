using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.TensorAnalysis {

    public class LinearVectorFunction : IVectorFunction {

        Tensor1 A;
        Tensor2 B;

        public int M { get; private set; }

        public int N { get; private set; }

        public LinearVectorFunction(Tensor2 B) {
            this.B = B;
            A = Tensor1.Zeros(B.N);
            N = A.M;
            M = B.N;
        }

        public LinearVectorFunction(Tensor1 A, Tensor2 B) {
            this.B = B;
            this.A = A;
            N = A.M;
            M = B.N;
        }

        public Tensor1 Evaluate(Tensor1 x) {
            return A + B * x;
        }

        public (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 x) {
            Tensor1 xTilde = Evaluate(x);
            Tensor2 jacobian = B.Clone();
            return (xTilde, jacobian);
        }

        public (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 x) {
            (Tensor1 xTilde , Tensor2 jacobian) = EvaluateAndJacobian(x);
            Tensor3 hessian = Tensor3.Zeros(N,M,M);
            return (xTilde, jacobian, hessian);
        }
    }
}

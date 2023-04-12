using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Tensor {

    public class QuadraticVectorPolynomial : IVectorFunction {

        Tensor1 A;
        Tensor2 B;
        Tensor3 C;

        public int M { get; private set; }

        public int N { get; private set; }

        public QuadraticVectorPolynomial(Tensor3 C) {
            this.C = C;
            A = Tensor1.Zeros(C.N);
            B = Tensor2.Zeros(C.N);
            N = A.M;
            M = B.N;
        }

        public QuadraticVectorPolynomial(Tensor1 A, Tensor2 B, Tensor3 C) {
            this.B = B;
            this.A = A;
            this.C = C;
            N = A.M;
            M = B.N;
        }

        public Tensor1 Evaluate(Tensor1 x) {
            return A + B * x + x * C * x;
        }

        public (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 x) {
            Tensor1 xTilde = Evaluate(x);
            Tensor2 jacobian = B + x * C + C * x;
            return (xTilde, jacobian);
        }

        public (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 x) {
            (Tensor1 xTilde , Tensor2 jacobian) = EvaluateAndJacobian(x);
            Tensor3 hessian = 2 * C;
            return (xTilde, jacobian, hessian);
        }
    }
}

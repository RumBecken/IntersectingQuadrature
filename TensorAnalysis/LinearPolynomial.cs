using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensorAnalysis {
    public class LinearPolynomial : IScalarFunction {

        public double A;

        public Tensor1 B;

        public int M { get; private set; }

        public LinearPolynomial(double a, Tensor1 b) {
            A = a;
            B = b;
            M = b.M;
        }

        public double Evaluate(Tensor1 x) {
            return A + B * x;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            double evaluation = Evaluate(x);
            Tensor1 gradient = B.Clone();
            return (evaluation, gradient);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            Tensor2 hessian = Tensor2.Zeros(M);
            return (evaluation, gradient, hessian);
        }
    }
}

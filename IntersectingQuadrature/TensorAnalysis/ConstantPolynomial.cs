using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.TensorAnalysis {
    public class ConstantPolynomial : IScalarFunction {

        public double A;

        public int M { get; private set; }

        public ConstantPolynomial(double a, int M = 3) {
            A = a;
            this.M = M;
        }

        public double Evaluate(Tensor1 x) {
            return A;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            double evaluation = Evaluate(x);
            return (evaluation, Tensor1.Zeros(M));
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            Tensor2 hessian = Tensor2.Zeros(M);
            return (evaluation, gradient, hessian);
        }
    }
}

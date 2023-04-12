using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Tensor {
    public class QuadraticPolynomial : IScalarFunction {

        public double A;

        public Tensor1 B;

        public Tensor2 C;

        public int M { get; private set; }

        public QuadraticPolynomial(double a, Tensor1 b, Tensor2 c) {
            A = a;
            B = b;
            C = c;
            M = b.M;
        }

        public QuadraticPolynomial(Tensor2 c) {
            A = 0;
            B = Tensor1.Zeros(c.N);
            C = c;
            M = c.N;
        }

        public double Evaluate(Tensor1 x) {
            return A + B * x + x * C * x;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            double f = Evaluate(x);
            Tensor1 gradient = B + (Algebra.Transpose(C) + C) * x;
            return (f, gradient);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            (double f, Tensor1 g) = EvaluateAndGradient(x);
            Tensor2 hessian = (Algebra.Transpose(C) + C);
            return (f, g, hessian);
        }
    }
}

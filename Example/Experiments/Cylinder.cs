using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example.Experiments {
    internal class Cylinder : IScalarFunction {

        double radius;
        Tensor1 center;

        public int M => 3;

        public Cylinder(Tensor1 center, double radius) {
            this.radius = radius;
            this.center = center;
        }

        public double Evaluate(Tensor1 x) {
            double xl = x[0] - center[0];
            double yl = x[1] - center[1];
            double f = Algebra.Pow(xl, 2) + Algebra.Pow(yl, 2) - Algebra.Pow(radius, 2);
            return f;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            double xl = x[0] - center[0];
            double yl = x[1] - center[1];
            Tensor1 gradient = Tensor1.Vector(xl * 2, yl * 2, 0);
            return (Evaluate(x), gradient);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            Tensor2 hessian = Tensor2.Zeros(3,3);
            hessian[0, 0] = 2;
            hessian[1, 1] = 2;
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            return (evaluation, gradient, hessian);
        }
    }
}

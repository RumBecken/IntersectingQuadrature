using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using TensorAnalysis;

namespace Example.Experiments {
    internal class Sphere : IScalarFunction {
        Tensor1 center;
        double radius;

        public int M { get; private set; }

        public Sphere(Tensor1 center, double radius){
            this.center = center;
            this.radius = radius;
            M = center.M;
        }

        public double Evaluate(Tensor1 x) {
            Tensor1 y = x - center;
            return y * y - radius * radius;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            double evaluation = Evaluate(x);
            Tensor1 gradient = 2 * ( x - center);
            return (evaluation, gradient);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            Tensor2 hessian = Tensor2.Zeros(M,M);
            for(int i= 0; i < M; ++i) {
                hessian[i, i] = 2;
            }
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            return (evaluation, gradient, hessian);
        }
    }
}

using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature {
    class GradientComponent : IScalarFunction {

        IScalarFunction alpha;

        Tensor1 normal;

        public GradientComponent(IScalarFunction alpha, int h) {
            this.alpha = alpha;
            normal = Tensor1.Zeros(alpha.M);
            normal[h] = 1;
        }

        public int M => alpha.M;

        public double Evaluate(Tensor1 x) {
            (double evaluation, Tensor1 gradient) = alpha.EvaluateAndGradient(x);
            return gradient * normal;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            (double evaluation, Tensor1 gradient, Tensor2 hessian)  = alpha.EvaluateAndGradientAndHessian(x);
            return (gradient * normal, hessian * normal);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            Console.WriteLine("Attention: Hack fix active");
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            double d = 0.000001;
            Tensor2 hessian = Tensor2.Zeros(x.M);
            for(int i = 0; i < x.M; ++i) {
                Tensor1 delta = Tensor1.Zeros(x.M);
                delta[i] = d;
                (double evaluation1, Tensor1 gradient1) = EvaluateAndGradient(x + delta);
                for (int j = 0; j < x.M; ++j) {
                    hessian[i, j] = (gradient1[j] - gradient[j]) / d; 
                }
            }

            return (evaluation, gradient, hessian);
        }
    }
}

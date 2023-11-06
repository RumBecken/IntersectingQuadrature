using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Tensor
{
    public class GradientComponent : IScalarFunction {

        IScalarFunction alpha;

        Tensor1 normal;

        public GradientComponent(IScalarFunction alpha, int component) {
            this.alpha = alpha;
            normal = Tensor1.Zeros(alpha.M);
            normal[component] = 1;
        }

        public int M => alpha.M;

        public double Evaluate(Tensor1 x) {
            (double evaluation, Tensor1 gradient) = alpha.EvaluateAndGradient(x);
            return gradient * normal;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            (double evaluation, Tensor1 gradient, Tensor2 hessian) = alpha.EvaluateAndGradientAndHessian(x);
            return (normal * gradient, normal * hessian);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            double d = 1e-8;
            Tensor2 hessian = Tensor2.Zeros(x.M);
            for (int i = 0; i < x.M; ++i)
            {
                Tensor1 delta = Tensor1.Zeros(x.M);
                delta[i] = d;
                double sign = -Math.Sign(x[i]);
                if (sign == 0)
                {
                    sign = 1;
                }
                (double evaluation1, Tensor1 gradient1) = EvaluateAndGradient(x + sign * delta);
                for (int j = 0; j < x.M; ++j)
                {
                    hessian[i, j] = sign * (gradient1[j] - gradient[j]) / d;
                }
            }

            return (evaluation, gradient, hessian);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example.Experiments.Shapes
{
    internal class Torus : IScalarFunction
    {

        double r;
        double R;
        Tensor1 center;

        public int M => 3;

        public Torus(Tensor1 center, double torusRadius, double tubeRadius)
        {
            R = torusRadius;
            r = tubeRadius;
            this.center = center;
        }

        public double Evaluate(Tensor1 x)
        {
            double xl = x[0] - center[0];
            double yl = x[1] - center[1];
            double zl = x[2] - center[2];

            double f = Math.Sqrt(Algebra.Pow(xl, 2) + Algebra.Pow(yl, 2)) - R;
            f = Algebra.Pow(f, 2) + Algebra.Pow(zl, 2) - Algebra.Pow(r, 2);
            return f;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x)
        {
            double xl = x[0] - center[0];
            double yl = x[1] - center[1];
            double zl = x[2] - center[2];

            double sqrxy = Math.Sqrt(Algebra.Pow(xl, 2) + Algebra.Pow(yl, 2));

            double dx = 2 * xl * (1 - R / sqrxy);
            double dy = 2 * yl * (1 - R / sqrxy);
            double dz = 2 * zl;

            Tensor1 gradient = Tensor1.Vector(dx, dy, dz);
            return (Evaluate(x), gradient);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x)
        {
            double xl = x[0] - center[0];
            double yl = x[1] - center[1];
            double zl = x[2] - center[2];

            double sqrxy = Math.Pow(Algebra.Pow(xl, 2) + Algebra.Pow(yl, 2), 1.5);
            Tensor2 hessian = Tensor2.Zeros(3, 3);

            //x
            hessian[0, 0] = 2 - 2 * R * yl * yl / sqrxy;
            hessian[0, 1] = 2 * R * xl * yl / sqrxy;
            hessian[0, 2] = 0;

            //y
            hessian[1, 0] = 2 * R * xl * yl / sqrxy;
            hessian[1, 1] = 2 - 2 * R * xl * xl / sqrxy;
            hessian[1, 2] = 0;

            //z
            hessian[2, 0] = 0;
            hessian[2, 1] = 0;
            hessian[2, 2] = 2;
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            return (evaluation, gradient, hessian);
        }
    }
}

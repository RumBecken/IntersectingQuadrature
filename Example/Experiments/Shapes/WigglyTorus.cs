using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example.Experiments.Shapes
{
    internal class WigglyCylinder : IScalarFunction
    {

        double r;
        double w;
        double p = 6;
        Tensor1 center;

        public int M => 3;

        public WigglyCylinder(Tensor1 center, double radius, double wiggle)
        {
            r = radius;
            w = wiggle;
            this.center = center;
            p = Math.PI;
        }

        public double Evaluate(Tensor1 x)
        {
            double xl = x[0] - center[0];
            double yl = x[1] - center[1];
            double zl = x[2] - center[2];
            double wy = w * Math.Sin(p * zl);
            double f = Algebra.Pow(xl, 2) + Algebra.Pow(yl + wy, 2) - r * r;
            return f;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x)
        {
            double xl = x[0] - center[0];
            double yl = x[1] - center[1];
            double zl = x[2] - center[2];

            double wy = w * Math.Sin(p * zl);
            double dx = 2 * xl;
            double dy = 2 * (yl + wy);
            double dz = 2 * (yl + wy) * w * Math.Cos(p * zl) * p;

            Tensor1 gradient = Tensor1.Vector(dx, dy, dz);
            return (Evaluate(x), gradient);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x)
        {
            double xl = x[0] - center[0];
            double yl = x[1] - center[1];
            double zl = x[2] - center[2];

            Tensor2 hessian = Tensor2.Zeros(3, 3);

            //x
            hessian[0, 0] = 2;
            hessian[0, 1] = 0;
            hessian[0, 2] = 0;

            //y
            hessian[1, 0] = 0;
            hessian[1, 1] = 2;
            hessian[1, 2] = 2 * w * Math.Cos(p * zl) * p;

            //z
            double wy = w * Math.Sin(p * zl);
            hessian[2, 0] = 0;
            hessian[2, 1] = 2 * w * Math.Cos(p * zl) * p;
            hessian[2, 2] = -2 * (yl + wy) * w * Math.Sin(p * zl) * p * p;
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            return (evaluation, gradient, hessian);
        }
    }
}

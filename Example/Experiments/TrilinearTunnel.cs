using IntersectingQuadrature;
using IntersectingQuadrature.TensorAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Experiments {
    internal class TrilinearTunnel {


        class Tunnel : IScalarFunction {
            public int M => 3;

            public double Evaluate(Tensor1 X) {
                double x = X[0];
                double y = X[1];
                double z = X[2];
                return 0.5 - 1.4*z + 2.9*x*y - 6.5 *x*y*z + 3.2*x*z - 1.2 *x + 3.3 *y*z - 1.3 *y;
            }

            public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 X) {
                double x = X[0];
                double y = X[1];
                double z = X[2];
                double dx = -1.2 + y*(2.9 - 6.5 * z) + 3.2 * z;
                double dy = -1.3 + x*(2.9 - 6.5 * z) + 3.3 * z;
                double dz = -1.4 + x * (3.2 - 6.5 * y) + 3.3 * y;
                return (Evaluate(X),Tensor1.Vector(dx,dy,dz));
            }

            public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 X) {
                double x = X[0];
                double y = X[1];
                double z = X[2];
                Tensor2 hessian = Tensor2.Zeros(3);
                hessian[0, 1] = 2.9 - 6.5 * z;
                hessian[0, 2] = -6.5 * y + 3.2;
                hessian[1, 0] = hessian[0, 1];
                hessian[1, 2] = -6.5 * x + 3.3;
                hessian[2, 0] = hessian[0, 2];
                hessian[2, 1] = -6.5 * x + 3.3;

                (double evaluation, Tensor1 gradient) = EvaluateAndGradient(X);
                return (evaluation, gradient, hessian);
            }
        }

        public static void Exact() {
            int n = 2;

            Tensor2 C = Tensor2.Zeros(3);
            C[1, 2] = -6.5; 
            IScalarFunction alpha = new QuadraticPolynomial(-1.2, Tensor1.Vector(0, 2.9, 3.2), C);
            IScalarFunction beta = new Tunnel();
            IScalarFunction f = new ConstantPolynomial(1);

            Quadrater finder = new Quadrater();
            HyperRectangle cube = new UnitCube(3);
            cube.Diameters = Tensor1.Vector(1,1,1);
            cube.Center = Tensor1.Vector(0.5, 0.5, 0.5);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, beta, Symbol.Zero, cube, n);

            IO.Write("nodes.txt", rule);
            double s = Math.Abs(Quadrature.Evaluate(f, rule));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    internal class NewtonMethod : IRootFinder {

        readonly double epsilon;

        public NewtonMethod(double epsilon) {
            this.epsilon = epsilon;
        }

        public Tensor1 Root(IScalarFunction phi, Tensor1 a, Tensor1 b) {
            double x1 = 0.5;

            Tensor1 dv0 = b - a;

            double minPhi = int.MaxValue;
            int minCounter = 0;
            double X = -1;
            double stepSize = 1;

            int iterationCounter = 0;
            while (minCounter < 5) {
                ++iterationCounter;
                double x0 = x1;
                Tensor1 v0 = (1 - x0) * a + x0 * b;
                (double phi0, Tensor1 gradient0) = phi.EvaluateAndGradient(v0);
                
                ++minCounter;
                if (Math.Abs(phi0) < minPhi) {
                    minPhi = Math.Abs(phi0);
                    minCounter = 0;
                    X = x0;
                } else {
                    stepSize = 0.2;
                }
                double dxPhi = gradient0 * dv0;
                double step = phi0 / dxPhi;
                x1 = x0 - stepSize * step;
                x1 = Math.Min(Math.Max(x1, 0), 1);

                if (Double.IsNaN(x1)) {
                    throw new Exception("Ill-posed root");
                }
            };
            //Console.WriteLine(iterationCounter);

            if (minPhi > epsilon) {
                throw new Exception("Root not found");
            }
            Tensor1 root = (1 - X) * a + X * b;
            return root;
        }
    }
}

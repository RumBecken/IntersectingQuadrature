using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature {
    internal class NewtonMethod : IRootFinder {

        readonly double epsilon;

        public NewtonMethod(double epsilon) {
            this.epsilon = epsilon;
        }

        public Tensor1 Root(IScalarFunction phi, Tensor1 a, Tensor1 b) {
            double x0 = default;
            double x1 = 0.5;

            Tensor1 dv0 = b - a;
            
            double minPhi = int.MaxValue;
            int minCounter = 0;
            double X = -1;
            while (minCounter < 2) {
                
                x0 = x1;
                Tensor1 v0 = (1 - x0) * a + x0 * b;
                (double phi0, Tensor1 gradient0) = phi.EvaluateAndGradient(v0);
                x1 = x0 - phi0 / (gradient0 * dv0);

                ++minCounter;
                if (Math.Abs(phi0) < minPhi) {
                    minPhi = Math.Abs(phi0);
                    minCounter = 0;
                    X = x0;
                }
            };
            
            
            if (minPhi > epsilon) {
                throw new Exception("Root not found");
            }
            /*
            if (X > 1 + epsilon) {
                X = 1;
            }
            if(X < -epsilon * 1e5) {
                X = 0;
            }
            //*/
            Tensor1 root = (1 - X) * a + X * b;
            return root;
        }
    }
}

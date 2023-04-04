using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using TensorAnalysis;

namespace Example.Experiments {
    internal class ParallelLines : IScalarFunction {
        
        public int M => 3;

        double c;
        double m;

        public ParallelLines(double gap, double slope) {
            this.c = gap;
            this.m = slope;
        }

         public double Evaluate(Tensor1 X) {
            double x = X[0];
            double y = X[1];
            return (m * x - y + c) * (m * x -  y - c);
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 X) {
            Tensor1 jacobian = Tensor1.Zeros(M);
            double x = X[0];
            double y = X[1];
            jacobian[0] = 2 * (m * x -  y) * m;
            jacobian[1] = -2 * (m * x -  y);
            return (Evaluate(X), jacobian);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 X) {
            Tensor2 hessian = Tensor2.Zeros(M);
            hessian[0, 0] = 2 * m* m;
            hessian[0, 1] = 2 * -m;
            hessian[1, 0] = -2 * m;
            hessian[1, 1] = 2 ;
            (double evaluation, Tensor1 jacobian) = EvaluateAndGradient(X); 
            return (evaluation, jacobian, hessian);
        }
    }
}

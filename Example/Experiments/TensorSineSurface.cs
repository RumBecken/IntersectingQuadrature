using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using TensorAnalysis;

namespace Example.Experiments {
    internal class TensorSineSurface : IScalarFunction {
        
        public int M => 3;

        double amplitude;

        double w;

        public TensorSineSurface(double amplitude, double period ) {
            this.amplitude = amplitude;
            w = 2 * Math.PI / period ;
        }

         public double Evaluate(Tensor1 x) {
            return x[2] - amplitude * Math.Sin(w * x[0])  * Math.Sin(w * x[1]);
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            Tensor1 jacobian = Tensor1.Zeros(M);
            jacobian[0] = - amplitude * w * Math.Cos(w * x[0])  * Math.Sin(w * x[1]);
            jacobian[1] = -amplitude * Math.Sin(w * x[0]) *  w * Math.Cos(w * x[1]);
            jacobian[2] = 1;
            return (Evaluate(x), jacobian);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            Tensor2 hessian = Tensor2.Zeros(M);
            hessian[0, 0] = amplitude * w * w * Math.Sin(w * x[0]) *  Math.Sin(w * x[1]);
            hessian[0, 1] = -amplitude * w * Math.Cos(w * x[0]) *  w * Math.Cos(w * x[1]);
            hessian[1, 0] = hessian[0, 1];
            hessian[1, 1] = amplitude * Math.Sin(w * x[0]) * w * w * Math.Sin(w * x[1]);
            (double evaluation, Tensor1 jacobian) = EvaluateAndGradient(x); 
            return (evaluation, jacobian, hessian);
        }
    }
}

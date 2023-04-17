using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Experiments {
    class Sheet : IScalarFunction {

        IScalarFunction f;

        public int M => f.M;

        double d;

        public Sheet(IScalarFunction f, double d) {
            this.f = f;
            this.d = d;
        }

        public double Evaluate(Tensor1 x) {
            double fx = f.Evaluate(x);
            return fx * fx - d * d;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            (double fx, Tensor1 gradfx) = f.EvaluateAndGradient(x);
            return (fx * fx - d * d, 2 * fx * gradfx);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            (double fx, Tensor1 gradfx, Tensor2 hessian) = f.EvaluateAndGradientAndHessian(x);
            Algebra.Scale(hessian, 2 * fx);
            Tensor2 hs = Algebra.Dyadic(gradfx, gradfx);
            Algebra.Scale(hs, 2);
            return (fx * fx - d * d, 2 * fx * gradfx, hessian + hs);
        }
    }
}

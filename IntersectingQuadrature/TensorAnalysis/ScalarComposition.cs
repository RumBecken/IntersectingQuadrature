using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.TensorAnalysis {
    public class ScalarComposition : IScalarFunction {

        IScalarFunction f;
        
        IVectorFunction injection;

        public int M { get; private set; }

        public ScalarComposition(IScalarFunction f, IVectorFunction injection) {
            this.f = f;
            this.injection = injection;
            
            Debug.Assert(f.M == injection.N);
            M = injection.M;
        }

        public double Evaluate(Tensor1 x) {
            Tensor1 tildeX = injection.Evaluate(x);
            return f.Evaluate(tildeX);
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            (Tensor1 i , Tensor2 iJacobian ) = injection.EvaluateAndJacobian(x);
            (double f, Tensor1 fGradient) = this.f.EvaluateAndGradient(i);

            Tensor1 gradient = fGradient * iJacobian; 
            return (f, gradient);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            (Tensor1 i, Tensor2 iJacobian, Tensor3 iHessian ) = injection.EvaluateAndJacobianAndHessian(x);
            (double f, Tensor1 fGradient, Tensor2 fHessian ) = this.f.EvaluateAndGradientAndHessian(i);

            Tensor2 hessian = Algebra.Transpose(iJacobian) * fHessian * iJacobian + fGradient * iHessian;

            Tensor1 gradient = fGradient * iJacobian;
            return (f, gradient, hessian);
        }
    }
}

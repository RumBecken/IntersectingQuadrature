using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorAnalysis;

namespace IntersectingQuadrature.Mapper {
   
    internal interface INestedVectorFunction {

        public void Evaluate(Tensor1 tilde, Tensor1 evaluation);
        
        public double EvaluateAndDeterminant(Tensor1 tilde, Tensor1 evaluation);

        public void EvaluateAndJacobian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian);

        public void EvaluateAndJacobianAndHessian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian);
    }
}

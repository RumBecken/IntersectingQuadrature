using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map.Nested
{

    internal interface INestedVectorFunction
    {

        void Evaluate(Tensor1 tilde, Tensor1 evaluation);

        double EvaluateAndDeterminant(Tensor1 tilde, Tensor1 evaluation);

        void EvaluateAndJacobian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian);

        void EvaluateAndJacobianAndHessian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian);
    }
}

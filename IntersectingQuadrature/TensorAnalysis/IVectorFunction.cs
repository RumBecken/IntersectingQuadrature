using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.TensorAnalysis {

    public interface IVectorFunction {
        int M { get; }

        int N { get; }

        Tensor1 Evaluate(Tensor1 x);

        (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 x);

        (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 x);
    }

}

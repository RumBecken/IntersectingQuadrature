using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensorAnalysis {

    public interface IVectorFunction {
        public int M { get; }

        public int N { get; }

        public Tensor1 Evaluate(Tensor1 x);

        public (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 x);

        public (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 x);
    }

}

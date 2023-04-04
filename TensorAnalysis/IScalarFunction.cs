using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensorAnalysis {
    public interface IScalarFunction {

        int M { get; }

        double Evaluate(Tensor1 x);

        (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x);

        (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x);
    }
}

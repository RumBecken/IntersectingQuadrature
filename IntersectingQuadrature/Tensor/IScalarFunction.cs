using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Tensor {
    
    /// <summary>
    /// Function that associates a scalar to a vector field
    /// </summary>
    public interface IScalarFunction {

        /// <summary>
        /// Dimension of the input vector field 
        /// </summary>
        int M { get; }

        /// <param name="x"><see cref="M"/>-dimensional argument of function</param>
        /// <returns>Value of the function at x</returns>
        double Evaluate(Tensor1 x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"><see cref="M"/>-dimensional argument of function</param>
        /// <returns>Value of the function and gradient vector at x</returns>
        (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"><see cref="M"/>-dimensional argument of function</param>
        /// <returns>Value of the function and gradient vector and hessian matrix at x</returns>
        (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x);
    }
}

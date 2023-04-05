using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature.Mapper {
    internal class ZeroLine : IHeightFunctionY {

        Tensor1 top;
        Tensor1 bottom;
        IRootFinder rooter;
        IScalarFunction alpha;

        public ZeroLine(IRootFinder rooter, IScalarFunction alpha, Tensor1 top, Tensor1 bottom) {
            this.rooter = rooter;
            this.alpha = alpha;
            this.top = top;
            this.bottom = bottom;
        }
        
        public double Y(double x) {
            Tensor1 xTop = top.Clone();
            xTop[0] = x;
            Tensor1 xBottom = bottom.Clone();
            xBottom[0] = x;
            Tensor1 y = rooter.Root(alpha, xBottom, xTop);
            return y[1];

        }

        public (double Y, double DxY) YdY(double x) {
            double y = Y(x);
            Tensor1 zero = top.Clone();
            zero[0] = x;
            zero[1] = y;
            (double e, Tensor1 gradient) = alpha.EvaluateAndGradient(zero);
            double dxy = -gradient[0] / gradient[1];
            return (y, dxy);
        }

        public (double Y, double DxY, double DxxY) YdYddY(double x) {
            double y = Y(x);
            Tensor1 zero = top.Clone();
            zero[0] = x;
            zero[1] = y;
            (double e, Tensor1 gradient, Tensor2 hessian) = alpha.EvaluateAndGradientAndHessian(zero);
            double dxy = -gradient[0] / gradient[1];

            double dxxy = -(hessian[0, 0] + 2 * hessian[0, 1] * dxy + hessian[1, 1] * dxy * dxy);
            dxxy /= gradient[1];
            return (y, dxy, dxxy);
        }
    }
}

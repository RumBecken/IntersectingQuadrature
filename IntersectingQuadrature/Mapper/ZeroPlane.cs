using TensorAnalysis;

namespace IntersectingQuadrature.Mapper {
    internal class ZeroPlane : IHeightFunctionZ {

        Tensor1 top;
        Tensor1 bottom;
        IRootFinder rooter;
        IScalarFunction alpha;

        public ZeroPlane(IRootFinder rooter, IScalarFunction alpha, Tensor1 top, Tensor1 bottom) {
            this.rooter = rooter;
            this.alpha = alpha;
            this.top = top;
            this.bottom = bottom;
        }
        
        public double Z(double x, double y) {
            Tensor1 xTop = top.Clone();
            xTop[0] = x;
            xTop[1] = y;

            Tensor1 xBottom = bottom.Clone(); 
            xBottom[0] = x;
            xBottom[1] = y;
            Tensor1 z = rooter.Root(alpha, xBottom, xTop);
            return z[2];
        }

        public (double Z, double DxZ, double DyZ) ZdZ(double x, double y) {
            double z = Z(x,y);
            Tensor1 zero = Tensor1.Vector(x, y, z);
            (double e, Tensor1 gradient) = alpha.EvaluateAndGradient(zero);
            double dxz = -gradient[0] / gradient[2];
            double dyz = -gradient[1] / gradient[2];
            return (z, dxz, dyz);
        }

        public (double Z, double DxZ, double DyZ, double DxxZ, double DxyZ, double DyyZ) ZdZddZ(double x, double y) {
            double z = Z(x, y);
            Tensor1 zero = Tensor1.Vector(x, y, z);
            (double e, Tensor1 gradient, Tensor2 hessian) = alpha.EvaluateAndGradientAndHessian(zero);
            double dxz = -gradient[0] / gradient[2];
            double dyz = -gradient[1] / gradient[2];

            double dxxz = -(hessian[0,0] + 2 * hessian[0,2] * dxz + hessian[2,2] * dxz * dxz);
            dxxz /= gradient[2];
            double dyyz = -(hessian[1, 1] + 2 * hessian[1, 2] * dyz + hessian[2, 2] * dyz * dyz);
            dyyz /= gradient[2];
            double dxyz = -(hessian[0, 1] + hessian[0, 2] * dyz + hessian[1, 2] * dxz + hessian[2, 2] * dxz * dyz);
            dxyz /= gradient[2];

            return (z, dxz, dyz, dxxz, dxyz, dyyz);
        }
    }
}

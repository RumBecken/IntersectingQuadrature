using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map.Nested
{

    internal class NestedMapping2 : INestedVectorFunction
    {

        protected IHeightFunctionY mPlus;

        protected IHeightFunctionY mMinus;

        NestedMapping1 nested;

        public NestedMapping2(NestedMapping1 nested, IHeightFunctionY mPlus, IHeightFunctionY mMinus)
        {
            this.mPlus = mPlus;
            this.mMinus = mMinus;
            this.nested = nested;
        }

        public void Evaluate(Tensor1 tilde, Tensor1 evaluation)
        {
            nested.Evaluate(tilde, evaluation);
            double x = evaluation[0];
            double mPlus = this.mPlus.Y(x);
            double mMinus = this.mMinus.Y(x);
            evaluation[1] = Recurse(mPlus, mMinus, tilde[1]) / 2;
        }

        public double EvaluateAndDeterminant(Tensor1 tilde, Tensor1 evaluation)
        {
            double J = nested.EvaluateAndDeterminant(tilde, evaluation);
            double x = evaluation[0];
            double mPlus = this.mPlus.Y(x);
            double mMinus = this.mMinus.Y(x);
            evaluation[1] = Recurse(mPlus, mMinus, tilde[1]) / 2;
            return J * (mPlus - mMinus) / 2;
        }

        public void EvaluateAndJacobian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian)
        {
            nested.EvaluateAndJacobian(tilde, evaluation, jacobian);
            double x = evaluation[0];
            (double mPlus, double dxmPlus) = this.mPlus.YdY(x);
            (double mMinus, double dxmMinus) = this.mMinus.YdY(x);
            evaluation[1] = Recurse(mPlus, mMinus, tilde[1]) / 2;

            jacobian[0, 1] = 0;
            double Mxy = Recurse(dxmPlus, dxmMinus, tilde[1]);
            jacobian[1, 0] = Mxy * jacobian[0, 0] / 2;
            jacobian[1, 1] = (mPlus - mMinus) / 2;
        }

        public void EvaluateAndJacobianAndHessian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian)
        {
            nested.EvaluateAndJacobian(tilde, evaluation, jacobian);
            double x = evaluation[0];
            (double mPlus, double dxmPlus, double dxxmPlus) = this.mPlus.YdYddY(x);
            (double mMinus, double dxmMinus, double dxxmMinus) = this.mMinus.YdYddY(x);
            evaluation[1] = Recurse(mPlus, mMinus, tilde[1]) / 2;

            jacobian[0, 1] = 0;
            double Mxy = Recurse(dxmPlus, dxmMinus, tilde[1]);
            jacobian[1, 0] = Mxy * jacobian[0, 0] / 2;
            jacobian[1, 1] = (mPlus - mMinus) / 2;

            double Mxxy = Recurse(dxxmPlus, dxxmMinus, tilde[1]) * jacobian[0, 0];
            hessian[1, 0, 0] = Mxxy * jacobian[0, 0] / 2;
            hessian[1, 0, 1] = (dxmPlus - dxmMinus) * jacobian[0, 0] / 2;
            hessian[1, 1, 0] = (dxmPlus - dxmMinus) * jacobian[0, 0] / 2;
            hessian[1, 1, 1] = 0;

            hessian[0, 0, 1] = 0;
            hessian[0, 1, 0] = 0;
            hessian[0, 1, 1] = 0;
        }

        static double Recurse(double mPlus, double mMinus, double variable)
        {
            double m = (mPlus - mMinus) * variable + mPlus + mMinus;
            return m;
        }
    }
}

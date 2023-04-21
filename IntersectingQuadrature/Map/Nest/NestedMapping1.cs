using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map.Nested
{

    internal class NestedMapping1 : INestedVectorFunction
    {

        protected IHeightFunctionX mPlus;

        protected IHeightFunctionX mMinus;

        public NestedMapping1(IHeightFunctionX mPlus, IHeightFunctionX mMinus)
        {
            this.mPlus = mPlus;
            this.mMinus = mMinus;
        }

        public void Evaluate(Tensor1 tilde, Tensor1 evaluation)
        {
            double mPlus = this.mPlus.X();
            double mMinus = this.mMinus.X();
            evaluation[0] = Recurse(mPlus, mMinus, tilde[0]) / 2;
        }

        public double EvaluateAndDeterminant(Tensor1 tilde, Tensor1 evaluation)
        {
            double mPlus = this.mPlus.X();
            double mMinus = this.mMinus.X();
            evaluation[0] = Recurse(mPlus, mMinus, tilde[0]) / 2;
            return (mPlus - mMinus) / 2;
        }

        public void EvaluateAndJacobian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian)
        {
            double mPlus = this.mPlus.X();
            double mMinus = this.mMinus.X();
            evaluation[0] = Recurse(mPlus, mMinus, tilde[0]) / 2;
            jacobian[0, 0] = (mPlus - mMinus) / 2;
        }

        public void EvaluateAndJacobianAndHessian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian)
        {
            double mPlus = this.mPlus.X();
            double mMinus = this.mMinus.X();
            evaluation[0] = Recurse(mPlus, mMinus, tilde[0]) / 2;
            jacobian[0, 0] = (mPlus - mMinus) / 2;
            hessian[0, 0, 0] = 0;
        }

        static double Recurse(double mPlus, double mMinus, double variable)
        {
            double m = (mPlus - mMinus) * variable + mPlus + mMinus;
            return m;
        }
    }

}
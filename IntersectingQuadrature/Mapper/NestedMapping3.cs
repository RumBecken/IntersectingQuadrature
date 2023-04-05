using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature.Mapper {

    internal class NestedMapping3 : INestedVectorFunction {

        protected IHeightFunctionZ mPlus;

        protected IHeightFunctionZ mMinus;

        NestedMapping2 nested;

        public NestedMapping3(NestedMapping2 nested, IHeightFunctionZ mPlus, IHeightFunctionZ mMinus) {
            this.mPlus = mPlus;
            this.mMinus = mMinus;
            this.nested = nested;
        }

        public void Evaluate(Tensor1 tilde, Tensor1 evaluation) {
            nested.Evaluate(tilde, evaluation);
            double x = evaluation[0];
            double y = evaluation[1];
            double mPlus = this.mPlus.Z(x,y);
            double mMinus = this.mMinus.Z(x,y);
            evaluation[2] = Recurse(mPlus, mMinus, tilde[2]) / 2;
        }

        public double EvaluateAndDeterminant(Tensor1 tilde, Tensor1 evaluation) {
            double J = nested.EvaluateAndDeterminant(tilde, evaluation);
            double x = evaluation[0];
            double y = evaluation[1];
            double mPlus = this.mPlus.Z(x, y);
            double mMinus = this.mMinus.Z(x, y);
            evaluation[2] = Recurse(mPlus, mMinus, tilde[2]) / 2;
            return J * (mPlus - mMinus) / 2;
        }

        public void EvaluateAndJacobian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian) {
            nested.EvaluateAndJacobian(tilde, evaluation, jacobian);
            double x = evaluation[0];
            double y = evaluation[1];
            (double mPlus, double dxmPlus, double dymPlus) = this.mPlus.ZdZ(x, y);
            (double mMinus, double dxmMinus, double dymMinus) = this.mMinus.ZdZ(x, y);
            evaluation[2] = Recurse(mPlus, mMinus, tilde[2]) / 2;

            double Mxz = Recurse(dxmPlus, dxmMinus, tilde[2]);
            double Myz = Recurse(dymPlus, dymMinus, tilde[2]);
            jacobian[0, 2] = 0;
            jacobian[1, 2] = 0;
            jacobian[2, 0] = (Mxz * jacobian[0, 0] + Myz * jacobian[1, 0]) / 2;
            jacobian[2, 1] = (Myz * jacobian[1, 1]) / 2;
            jacobian[2, 2] = (mPlus - mMinus) / 2;
        }

        public void EvaluateAndJacobianAndHessian(Tensor1 tilde, Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) {
            nested.EvaluateAndJacobian(tilde, evaluation, jacobian);
            double x = evaluation[0];
            double y = evaluation[1];
            (double mPlus, double dxmPlus, double dymPlus, double dxxmPlus, double dxymPlus, double dyymPlus) = this.mPlus.ZdZddZ(x, y);
            (double mMinus, double dxmMinus, double dymMinus, double dxxmMinus, double dxymMinus, double dyymMinus) = this.mMinus.ZdZddZ(x, y);
            evaluation[2] = Recurse(mPlus, mMinus, tilde[2]) / 2;

            double Mxz = Recurse(dxmPlus, dxmMinus, tilde[2]);
            double Myz = Recurse(dymPlus, dymMinus, tilde[2]);
            jacobian[0, 2] = 0;
            jacobian[1, 2] = 0;
            jacobian[2, 0] = (Mxz * jacobian[0, 0] + Myz * jacobian[1, 0]) / 2;
            jacobian[2, 1] = (Myz * jacobian[1, 1]) / 2;
            jacobian[2, 2] = (mPlus - mMinus) / 2;

            double Mxxz = Recurse(dxxmPlus, dxxmMinus, tilde[2]) * jacobian[0, 0] + Recurse(dxymPlus, dxymMinus, tilde[2]) * jacobian[1, 0];
            double Mxyz = Recurse(dxymPlus, dxymMinus, tilde[2]) * jacobian[0, 0] + Recurse(dyymPlus, dyymMinus, tilde[2]) * jacobian[1, 0];
            double Myyz = Recurse(dxymPlus, dxymMinus, tilde[2]) * jacobian[0, 1] + Recurse(dyymPlus, dyymMinus, tilde[2]) * jacobian[1, 1];
            hessian[2, 0, 0] = (Mxxz * jacobian[0, 0] + Mxyz * jacobian[1, 0] + Myz * hessian[1, 0, 0]) / 2;
            hessian[2, 0, 1] = (Mxyz * jacobian[1, 1] + Myz * hessian[1, 0, 1]) / 2;
            hessian[2, 0, 2] = ((dxmPlus - dxmMinus) * jacobian[0, 0] + (dymPlus - dymMinus) * jacobian[1, 0]) / 2;
            hessian[2, 1, 0] = (Mxyz * jacobian[1, 1] + Myz * hessian[1, 0, 1]) / 2;
            hessian[2, 1, 1] = Myyz * jacobian[1, 1] / 2;
            hessian[2, 1, 2] = (dymPlus - dymMinus) * jacobian[1, 1] / 2;
            hessian[2, 2, 0] = ((dxmPlus - dxmMinus) * jacobian[0, 0] + (dymPlus - dymMinus) * jacobian[1, 0]) / 2;
            
            hessian[2, 2, 1] = (dymPlus - dymMinus) * jacobian[1, 1] / 2;
            hessian[2, 2, 2] = 0;

            hessian[1, 0, 2] = 0;
            hessian[1, 1, 2] = 0;
            hessian[1, 2, 0] = 0;
            hessian[1, 2, 1] = 0;
            hessian[1, 2, 2] = 0;

            hessian[0, 0, 2] = 0;
            hessian[0, 1, 2] = 0;
            hessian[0, 2, 0] = 0;
            hessian[0, 2, 1] = 0;
            hessian[0, 2, 2] = 0;
        }

        static double Recurse(double mPlus, double mMinus, double variable) {
            double m = (mPlus - mMinus) * variable + mPlus + mMinus;
            return m;
        }
    }
}

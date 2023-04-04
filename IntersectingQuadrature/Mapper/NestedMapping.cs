using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorAnalysis;

namespace IntersectingQuadrature.Mapper {

    internal class NestedMapping : IIntegralMapping {
        int d;

        public int M => d;

        public int N => d;

        INestedVectorFunction map;

        private NestedMapping(int d, INestedVectorFunction map) {
            this.d = d;
            this.map = map;
        }

        public static NestedMapping Dimension1(Heights mPlus, Heights mMinus) {
            NestedMapping1 map1 = new NestedMapping1(mPlus.x, mMinus.x);
            return new NestedMapping(1, map1);
        }

        public static NestedMapping Dimension2(Heights mPlus, Heights mMinus) {
            NestedMapping1 map1 = new NestedMapping1(mPlus.x, mMinus.x);
            NestedMapping2 map2 = new NestedMapping2(map1, mPlus.y, mMinus.y);
            return new NestedMapping(2, map2);
        }

        public static NestedMapping Dimension3(Heights mPlus, Heights mMinus) {
            NestedMapping1 map1 = new NestedMapping1(mPlus.x, mMinus.x);
            NestedMapping2 map2 = new NestedMapping2(map1, mPlus.y, mMinus.y);
            NestedMapping3 map3 = new NestedMapping3(map2, mPlus.z, mMinus.z);
            return new NestedMapping(3, map3);
        }

        public static NestedMapping Dimension(int dim, Heights mPlus, Heights mMinus) {
            switch (dim) {
                case 1:
                return Dimension1(mPlus, mMinus);
                case 2:
                return Dimension2(mPlus, mMinus);
                case 3:
                return Dimension3(mPlus, mMinus);
                default:
                throw new NotImplementedException();
            }
        }

        public Tensor1 Evaluate(Tensor1 tilde) {
            Tensor1 evaluation = Tensor1.Zeros(d);
            map.Evaluate(tilde, evaluation);
            return evaluation;
        }

        public (double J, Tensor1 X) EvaluateAndDeterminant(Tensor1 tilde) {
            Tensor1 evaluation = Tensor1.Zeros(d);
            double J = map.EvaluateAndDeterminant(tilde, evaluation);
            return (J, evaluation);
        }

        public (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 tilde) {
            Tensor1 evaluation = Tensor1.Zeros(d);
            Tensor2 jacobian = Tensor2.Zeros(d);
            map.EvaluateAndJacobian(tilde, evaluation, jacobian);
            return (evaluation, jacobian);
        }

        public (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 tilde) {
            Tensor1 evaluation = Tensor1.Zeros(d);
            Tensor2 jacobian = Tensor2.Zeros(d);
            Tensor3 hessian = Tensor3.Zeros(d);
            map.EvaluateAndJacobianAndHessian(tilde, evaluation, jacobian, hessian);
            return (evaluation, jacobian, hessian);
        }
    }
}

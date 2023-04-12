using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature {
    class Restriction : IVectorFunction {

        int m;

        int n;

        public int M => m;

        public int N => n;

        LinearVectorPolynomial affineProjection;

        public Restriction(HyperRectangle face) {
            m = face.SpaceDimension;
            n = face.SpaceDimension;
            Tensor2 projection = Tensor2.Zeros(m);
            Tensor1 affine = Tensor1.Zeros(m);

            for (int i = 0; i < m; ++i) {
                if (face.ActiveDimensions[i]) {
                    projection[i, i] = 1;
                } else {
                    affine[i] = face.Center[i];
                }
            }
            affineProjection = new LinearVectorPolynomial(affine, projection);
        }

        public Tensor1 Evaluate(Tensor1 x) {
            return affineProjection.Evaluate(x);
        }

        public (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 x) {
            return affineProjection.EvaluateAndJacobian(x);
        }

        public (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 x) {
            return affineProjection.EvaluateAndJacobianAndHessian(x);
        }
    }
}

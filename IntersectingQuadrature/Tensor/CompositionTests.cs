using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace IntersectingQuadrature.Tensor {
    internal class CompositionTests {

        [Test]
        public static void ScalarCompositionHessian() {

            IScalarFunction f = new QuadraticPolynomial(Tensor2.Unit(2));
            Tensor3 C = Tensor3.Zeros(2);
            C[0, 0, 0] = 1;
            C[1, 1, 1] = 1;
            IVectorFunction A = new QuadraticVectorPolynomial(C);

            ScalarComposition t = new ScalarComposition(f, A);
            double evaluation; 
            Tensor1 gradient; 
            Tensor2 hessian;
            (evaluation, gradient, hessian) = t.EvaluateAndGradientAndHessian(Tensor1.Vector(-1,-1));

            Assert.AreEqual(evaluation, 2, 1e-10);

            Assert.AreEqual(gradient[0], -4, 1e-10);
            Assert.AreEqual(gradient[1], -4, 1e-10);

            Assert.AreEqual(hessian[0, 0], 12, 1e-10);
            Assert.AreEqual(hessian[0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1], 12, 1e-10);
        }

        [Test]
        public static void ScalarCompositionHessianSkew() {

            IScalarFunction f = new QuadraticPolynomial(Tensor2.Unit(2));
            Tensor3 C = Tensor3.Zeros(2);
            C[0, 0, 1] = 1;
            C[1, 1, 0] = 1;
            IVectorFunction A = new QuadraticVectorPolynomial(C);

            ScalarComposition t = new ScalarComposition(f, A);
            double evaluation;
            Tensor1 gradient;
            Tensor2 hessian;
            double x = -1;
            double y = -3;
            Tensor1 X = Tensor1.Vector(x, y);
            (evaluation, gradient, hessian) = t.EvaluateAndGradientAndHessian(X);

            Assert.AreEqual(evaluation, 2* x * x * y * y, 1e-10);

            Assert.AreEqual(gradient[0], 4 * x * y * y, 1e-10);
            Assert.AreEqual(gradient[1], 4 * x * x * y, 1e-10);

            Assert.AreEqual(hessian[0, 0], 4 * y * y, 1e-10);
            Assert.AreEqual(hessian[0, 1], 8 * x * y, 1e-10);
            Assert.AreEqual(hessian[1, 0], 8 * x * y, 1e-10);
            Assert.AreEqual(hessian[1, 1], 4 * x * x, 1e-10);
        }

        [Test]
        public static void VectorCompositionHessian() {

            Tensor3 C = Tensor3.Zeros(2);
            C[0, 0, 0] = 1;
            C[1, 1, 1] = 1;
            IVectorFunction A = new QuadraticVectorPolynomial(C);

            VectorComposition t = new VectorComposition(A, A);
            Tensor1 evaluation;
            Tensor2 gradient;
            Tensor3 hessian;
            (evaluation, gradient, hessian) = t.EvaluateAndJacobianAndHessian(Tensor1.Vector(-1, -1));
            Assert.AreEqual(evaluation[0], 1, 1e-10);
            Assert.AreEqual(evaluation[1], 1, 1e-10);

            Assert.AreEqual(gradient[0,0], -4, 1e-10);
            Assert.AreEqual(gradient[0,1], 0, 1e-10);
            Assert.AreEqual(gradient[1,0], 0, 1e-10);
            Assert.AreEqual(gradient[1,1], -4, 1e-10);

            Assert.AreEqual(hessian[0, 0, 0], 12, 1e-10);
            Assert.AreEqual(hessian[0, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 1], 12, 1e-10);
        }

        [Test]
        public static void VectorCompositionHessianSkew() {

            Tensor3 C = Tensor3.Zeros(2);
            C[0, 0, 1] = 1;
            C[1, 1, 0] = 2;
            IVectorFunction A = new QuadraticVectorPolynomial(C);

            VectorComposition t = new VectorComposition(A, A);
            Tensor1 evaluation;
            Tensor2 gradient;
            Tensor3 hessian;

            double x = -1;
            double y = -3;
            Tensor1 X = Tensor1.Vector(x, y);
            (evaluation, gradient, hessian) = t.EvaluateAndJacobianAndHessian(X);
            Assert.AreEqual(evaluation[0], 2 * x * x * y * y, 1e-10);
            Assert.AreEqual(evaluation[1], 4 * x * x * y * y, 1e-10);

            Assert.AreEqual(gradient[0, 0], 4 * x * y * y, 1e-10);
            Assert.AreEqual(gradient[0, 1], 4 * x * x * y, 1e-10);
            Assert.AreEqual(gradient[1, 0], 8 * x * y * y, 1e-10);
            Assert.AreEqual(gradient[1, 1], 8 * x * x * y, 1e-10);

            Assert.AreEqual(hessian[0, 0, 0], 4 * y * y, 1e-10);
            Assert.AreEqual(hessian[0, 0, 1], 8 * x * y, 1e-10);
            Assert.AreEqual(hessian[0, 1, 0], 8 * x * y, 1e-10);
            Assert.AreEqual(hessian[0, 1, 1], 4 * x * x, 1e-10);
            Assert.AreEqual(hessian[1, 0, 0], 8 * y * y, 1e-10);
            Assert.AreEqual(hessian[1, 0, 1], 16 * x * y, 1e-10);
            Assert.AreEqual(hessian[1, 1, 0], 16 * x * y, 1e-10);
            Assert.AreEqual(hessian[1, 1, 1], 8 * x * x, 1e-10);
        }

    }
}

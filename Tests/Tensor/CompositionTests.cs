using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using IntersectingQuadrature.Tensor;

namespace Tests.Tensor
{
    internal class CompositionTests
    {

        [Test]
        public static void ScalarCompositionHessian()
        {

            IScalarFunction f = new QuadraticPolynomial(Tensor2.Unit(2));
            Tensor3 C = Tensor3.Zeros(2);
            C[0, 0, 0] = 1;
            C[1, 1, 1] = 1;
            IVectorFunction A = new QuadraticVectorPolynomial(C);

            ScalarComposition t = new ScalarComposition(f, A);
            double evaluation;
            Tensor1 gradient;
            Tensor2 hessian;
            (evaluation, gradient, hessian) = t.EvaluateAndGradientAndHessian(Tensor1.Vector(-1, -1));

            Assert.That(evaluation, Is.EqualTo(2).Within(1e-10));

            Assert.That(gradient[0], Is.EqualTo(-4).Within(1e-10));
            Assert.That(gradient[1], Is.EqualTo(-4).Within(1e-10));

            Assert.That(hessian[0, 0], Is.EqualTo(12).Within(1e-10));
            Assert.That(hessian[0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1], Is.EqualTo(12).Within(1e-10));
        }

        [Test]
        public static void ScalarCompositionHessianSkew()
        {

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

            Assert.That(evaluation, Is.EqualTo(2 * x * x * y * y).Within(1e-10));

            Assert.That(gradient[0], Is.EqualTo(4 * x * y * y).Within(1e-10));
            Assert.That(gradient[1], Is.EqualTo(4 * x * x * y).Within(1e-10));

            Assert.That(hessian[0, 0], Is.EqualTo(4 * y * y).Within(1e-10));
            Assert.That(hessian[0, 1], Is.EqualTo(8 * x * y).Within(1e-10));
            Assert.That(hessian[1, 0], Is.EqualTo(8 * x * y).Within(1e-10));
            Assert.That(hessian[1, 1], Is.EqualTo(4 * x * x).Within(1e-10));
        }

        [Test]
        public static void VectorCompositionHessian()
        {

            Tensor3 C = Tensor3.Zeros(2);
            C[0, 0, 0] = 1;
            C[1, 1, 1] = 1;
            IVectorFunction A = new QuadraticVectorPolynomial(C);

            VectorComposition t = new VectorComposition(A, A);
            Tensor1 evaluation;
            Tensor2 gradient;
            Tensor3 hessian;
            (evaluation, gradient, hessian) = t.EvaluateAndJacobianAndHessian(Tensor1.Vector(-1, -1));
            Assert.That(evaluation[0], Is.EqualTo(1).Within(1e-10));
            Assert.That(evaluation[1], Is.EqualTo(1).Within(1e-10));

            Assert.That(gradient[0, 0], Is.EqualTo(-4).Within(1e-10));
            Assert.That(gradient[0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(gradient[1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(gradient[1, 1], Is.EqualTo(-4).Within(1e-10));

            Assert.That(hessian[0, 0, 0], Is.EqualTo(12).Within(1e-10));
            Assert.That(hessian[0, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 1], Is.EqualTo(12).Within(1e-10));
        }

        [Test]
        public static void VectorCompositionHessianSkew()
        {

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
            Assert.That(evaluation[0], Is.EqualTo(2 * x * x * y * y).Within(1e-10));
            Assert.That(evaluation[1], Is.EqualTo(4 * x * x * y * y).Within(1e-10));

            Assert.That(gradient[0, 0], Is.EqualTo(4 * x * y * y).Within(1e-10));
            Assert.That(gradient[0, 1], Is.EqualTo(4 * x * x * y).Within(1e-10));
            Assert.That(gradient[1, 0], Is.EqualTo(8 * x * y * y).Within(1e-10));
            Assert.That(gradient[1, 1], Is.EqualTo(8 * x * x * y).Within(1e-10));

            Assert.That(hessian[0, 0, 0], Is.EqualTo(4 * y * y).Within(1e-10));
            Assert.That(hessian[0, 0, 1], Is.EqualTo(8 * x * y).Within(1e-10));
            Assert.That(hessian[0, 1, 0], Is.EqualTo(8 * x * y).Within(1e-10));
            Assert.That(hessian[0, 1, 1], Is.EqualTo(4 * x * x).Within(1e-10));
            Assert.That(hessian[1, 0, 0], Is.EqualTo(8 * y * y).Within(1e-10));
            Assert.That(hessian[1, 0, 1], Is.EqualTo(16 * x * y).Within(1e-10));
            Assert.That(hessian[1, 1, 0], Is.EqualTo(16 * x * y).Within(1e-10));
            Assert.That(hessian[1, 1, 1], Is.EqualTo(8 * x * x).Within(1e-10));
        }

    }
}

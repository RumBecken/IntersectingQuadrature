using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using IntersectingQuadrature.Tensor;

namespace Tests.Tensor
{
    internal class GradientComponentTest
    {
        [Test]
        public static void Quadratic()
        {
            IScalarFunction f = new QuadraticPolynomial(Tensor2.Unit(2));
            Tensor3 C = Tensor3.Zeros(2);
            C[0, 0, 0] = 1;
            C[1, 1, 1] = 1;
            IVectorFunction A = new QuadraticVectorPolynomial(C);
            ScalarComposition fA = new ScalarComposition(f, A);


            double x = -1;
            double y = 2;

            double evaluation;
            Tensor1 gradient;
            Tensor2 hessian;
            GradientComponent F = new GradientComponent(fA, 0);
            (evaluation, gradient, hessian) = F.EvaluateAndGradientAndHessian(Tensor1.Vector(x, y));

            Assert.That(evaluation, Is.EqualTo(4 * x * x * x).Within(1e-10));

            Assert.That(gradient[0], Is.EqualTo(12 * x * x).Within(1e-10));
            Assert.That(gradient[1], Is.EqualTo(0).Within(1e-10));

            Assert.That(24 * x, Is.EqualTo(hessian[0, 0]).Within(1e-6));
            Assert.That(0, Is.EqualTo(hessian[0, 1]).Within(1e-10));
            Assert.That(0, Is.EqualTo(hessian[1, 0]).Within(1e-10));
            Assert.That(0, Is.EqualTo(hessian[1, 1]).Within(1e-10));

            F = new GradientComponent(fA, 1);
            (evaluation, gradient, hessian) = F.EvaluateAndGradientAndHessian(Tensor1.Vector(x, y));

            Assert.That(evaluation, Is.EqualTo(4 * y * y * y).Within(1e-10));

            Assert.That(gradient[0], Is.EqualTo(0).Within(1e-10));
            Assert.That(gradient[1], Is.EqualTo(12 * y * y).Within(1e-10));

            Assert.That(hessian[0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1], Is.EqualTo(24 * y).Within(1e-6));
        }
    }
}

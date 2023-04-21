using IntersectingQuadrature.Tensor;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Map.Decompose
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

            Assert.AreEqual(evaluation, 4 * x * x * x, 1e-10);

            Assert.AreEqual(gradient[0], 12 * x * x, 1e-10);
            Assert.AreEqual(gradient[1], 0, 1e-10);

            Assert.AreEqual(hessian[0, 0], 24 * x, 1e-6);
            Assert.AreEqual(hessian[0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1], 0, 1e-10);

            F = new GradientComponent(fA, 1);
            (evaluation, gradient, hessian) = F.EvaluateAndGradientAndHessian(Tensor1.Vector(x, y));

            Assert.AreEqual(evaluation, 4 * y * y * y, 1e-10);

            Assert.AreEqual(gradient[0], 0, 1e-10);
            Assert.AreEqual(gradient[1], 12 * y * y, 1e-10);

            Assert.AreEqual(hessian[0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1], 24 * y, 1e-6);
        }
    }
}

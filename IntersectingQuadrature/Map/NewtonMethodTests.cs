using IntersectingQuadrature.Tensor;
using NUnit.Framework;
using System;

namespace IntersectingQuadrature.Map
{
    internal class NewtonMethodTests
    {

        class CubicLine : IScalarFunction
        {
            public int M => 1;

            public double Root { get; private set; }

            public CubicLine()
            {
                Root = 0.8;
            }

            public double Evaluate(Tensor1 X)
            {
                double x = X[0];
                double p = -0.2 * Algebra.Pow(x, 5) + 0.45 * Algebra.Pow(x, 4) - 0.32 * Algebra.Pow(x, 3) + 0.08 * Algebra.Pow(x, 2);
                double c = -0.2 * Algebra.Pow(Root, 5) + 0.45 * Algebra.Pow(Root, 4)
                    - 0.32 * Algebra.Pow(Root, 3) + 0.08 * Algebra.Pow(Root, 2);
                return p - c;


            }

            public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 X)
            {
                double x = X[0];
                double evaluation = Evaluate(X);
                Tensor1 gradient = Tensor1.Vector(-Algebra.Pow(x, 4)
                    + 1.8 * Algebra.Pow(x, 3) - 0.96 * Algebra.Pow(x, 2) + 0.16 * x);
                return (evaluation, gradient);
            }

            public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 X)
            {
                throw new NotImplementedException();
            }
        }


        [Test]
        public static void ZeroGradientOnBothBoundaries()
        {
            IRootFinder newton = new NewtonMethod(1e-10);

            CubicLine alpha = new CubicLine();

            Tensor1 a = Tensor1.Vector(0);
            Tensor1 b = Tensor1.Vector(1);

            Tensor1 numRoot = newton.Root(alpha, a, b);

            Assert.AreEqual(numRoot[0], alpha.Root, 1e-10);
        }

        [Test]
        public static void ZeroGradientOnLowerBoundary()
        {
            IRootFinder newton = new NewtonMethod(1e-10);
            CubicLine alpha = new CubicLine();

            Tensor1 a = Tensor1.Vector(0);
            Tensor1 b = Tensor1.Vector(0.9);
            Tensor1 numRoot = newton.Root(alpha, a, b);

            Assert.AreEqual(numRoot[0], alpha.Root, 1e-10);
        }

        [Test]
        public static void ZeroGradientOnUpperBoundary()
        {
            IRootFinder newton = new NewtonMethod(1e-10);
            CubicLine alpha = new CubicLine();

            Tensor1 a = Tensor1.Vector(0.2);
            Tensor1 b = Tensor1.Vector(1);
            Tensor1 numRoot = newton.Root(alpha, a, b);

            Assert.AreEqual(numRoot[0], alpha.Root, 1e-10);
        }

        [Test]
        public static void ZeroGradientOnRoot()
        {
            IRootFinder newton = new NewtonMethod(1e-10);
            Tensor2 A = Tensor2.Zeros(1);
            A[0, 0] = 1;
            QuadraticPolynomial alpha = new QuadraticPolynomial(0, Tensor1.Zeros(1), A);

            Tensor1 a = Tensor1.Vector(-1);
            Tensor1 b = Tensor1.Vector(1);
            Tensor1 numRoot = newton.Root(alpha, a, b);

            Assert.AreEqual(numRoot[0], 0, 1e-10);
        }

        [Test]
        public static void ZeroCloseAndZeroGradientAtLowerBoundary()
        {
            IRootFinder newton = new NewtonMethod(1e-10);
            double root = 0.01;
            Tensor2 A = Tensor2.Zeros(1);
            A[0, 0] = 1;
            QuadraticPolynomial alpha = new QuadraticPolynomial(-root * root, Tensor1.Vector(0), A);

            Tensor1 a = Tensor1.Vector(0);
            Tensor1 b = Tensor1.Vector(1);
            Tensor1 numRoot = newton.Root(alpha, a, b);

            Assert.AreEqual(numRoot[0], root, 1e-10);
        }
    }
}

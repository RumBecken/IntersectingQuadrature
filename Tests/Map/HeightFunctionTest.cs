using IntersectingQuadrature.Tensor;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using IntersectingQuadrature.Map;
using IntersectingQuadrature.Map.Nested;

namespace Tests.Map
{
    internal class HeightFunctionTest
    {

        [Test]
        public static void Y()
        {
            IRootFinder rooter = new NewtonMethod(1e-10);

            Tensor2 C = Tensor2.Unit(3);
            IScalarFunction alpha = new QuadraticPolynomial(-1, Tensor1.Zeros(3), C);

            IHeightFunctionY line = new ZeroLine(rooter, alpha, Tensor1.Vector(0, 1, 0), Tensor1.Vector(0, 0, 0));
            double x = 0.5;
            (double y, double dxY, double dxxY) = line.YdYddY(x);

            Assert.That(y, Is.EqualTo( Math.Sqrt(1 - x * x)).Within( 1e-10));
            Assert.That(dxY, Is.EqualTo(-x / y).Within(1e-10));
            Assert.That(dxxY, Is.EqualTo(-1.0 / Math.Pow(1 - x * x, 1.5)).Within(1e-10));
        }

        [Test]
        public static void Z()
        {
            IRootFinder rooter = new NewtonMethod(1e-10);

            Tensor2 C = Tensor2.Unit(3);
            IScalarFunction alpha = new QuadraticPolynomial(-1, Tensor1.Zeros(3), C);

            IHeightFunctionZ surface = new ZeroPlane(rooter, alpha, Tensor1.Vector(0, 0, 1), Tensor1.Vector(0, 0, 0));
            double x = 0.5;
            double y = 0.5;

            (double Z, double DxZ, double DyZ, double DxxZ, double DxyZ, double DyyZ) = surface.ZdZddZ(x, y);

            Assert.That(Z, Is.EqualTo(Math.Sqrt(1 - x * x - y * y)).Within(1e-10));
            Assert.That(DxZ, Is.EqualTo(-x / Z).Within(1e-10));
            Assert.That(DyZ, Is.EqualTo(-y / Z).Within(1e-10));
            Assert.That(DxxZ, Is.EqualTo((y * y - 1.0) / Math.Pow(1 - x * x - y * y, 1.5)).Within(1e-10));
            Assert.That(DxyZ, Is.EqualTo(-(x * y) / Math.Pow(1 - x * x - y * y, 1.5)).Within(1e-10));
            Assert.That(DyyZ, Is.EqualTo((x * x - 1.0) / Math.Pow(1 - x * x - y * y, 1.5)).Within(1e-10));
        }
    }
}

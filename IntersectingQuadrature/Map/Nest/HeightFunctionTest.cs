using IntersectingQuadrature.Tensor;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Map.Nested
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

            Assert.AreEqual(y, Math.Sqrt(1 - x * x), 1e-10);
            Assert.AreEqual(dxY, -x / y, 1e-10);
            Assert.AreEqual(dxxY, -1.0 / Math.Pow(1 - x * x, 1.5), 1e-10);
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

            Assert.AreEqual(Z, Math.Sqrt(1 - x * x - y * y), 1e-10);
            Assert.AreEqual(DxZ, -x / Z, 1e-10);
            Assert.AreEqual(DyZ, -y / Z, 1e-10);
            Assert.AreEqual(DxxZ, (y * y - 1.0) / Math.Pow(1 - x * x - y * y, 1.5), 1e-10);
            Assert.AreEqual(DxyZ, -(x * y) / Math.Pow(1 - x * x - y * y, 1.5), 1e-10);
            Assert.AreEqual(DyyZ, (x * x - 1.0) / Math.Pow(1 - x * x - y * y, 1.5), 1e-10);
        }
    }
}

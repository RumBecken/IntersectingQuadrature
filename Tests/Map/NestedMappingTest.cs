using IntersectingQuadrature.Tensor;
using IntersectingQuadrature.Map;
using IntersectingQuadrature.Map.Nested;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Map {
    internal class NestedMappingTest
    {

        static Heights Plane(double m)
        {
            Heights plane = new Heights();
            plane.SetX(new Point(m));
            plane.SetY(new Line(m));
            plane.SetZ(new Plane(m));

            return plane;
        }

        [Test]
        public static void Line()
        {
            NestedMapping m = NestedMapping.Dimension1(Plane(1), Plane(0));
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Zeros(1));

            Assert.That(evaluation[0], Is.EqualTo( 0.5).Within(1e-10));
            Assert.That(jacobian[0, 0], Is.EqualTo(0.5).Within(1e-10));
            Assert.That(hessian[0, 0, 0], Is.EqualTo(0).Within(1e-10));
        }

        [Test]
        public static void Square()
        {
            NestedMapping m = NestedMapping.Dimension2(Plane(1), Plane(0));
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Zeros(2));

            Assert.That(evaluation[0], Is.EqualTo(0.5).Within(1e-10));
            Assert.That(evaluation[1], Is.EqualTo(0.5).Within(1e-10));

            Assert.That(jacobian[0, 0], Is.EqualTo(0.5).Within(1e-10));
            Assert.That(jacobian[0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[1, 1], Is.EqualTo(0.5).Within(1e-10));

            Assert.That(hessian[0, 0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 1], Is.EqualTo(0).Within(1e-10));
        }

        [Test]
        public static void Cube()
        {
            NestedMapping m = NestedMapping.Dimension3(Plane(1), Plane(0));
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Zeros(3));

            Assert.That(evaluation[0], Is.EqualTo(0.5).Within(1e-10));
            Assert.That(evaluation[1], Is.EqualTo(0.5).Within(1e-10));
            Assert.That(evaluation[2], Is.EqualTo(0.5).Within(1e-10));

            Assert.That(jacobian[0, 0], Is.EqualTo(0.5).Within(1e-10));
            Assert.That(jacobian[0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[0, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[1, 1], Is.EqualTo(0.5).Within(1e-10));
            Assert.That(jacobian[1, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[2, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[2, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[2, 2], Is.EqualTo(0.5).Within(1e-10));

            Assert.That(hessian[0, 0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 0, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 2, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 2, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 2, 2], Is.EqualTo(0).Within(1e-10));

            Assert.That(hessian[1, 0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 2, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 2, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 2, 2], Is.EqualTo(0).Within(1e-10));

            Assert.That(hessian[2, 0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[2, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[2, 0, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[2, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[2, 1, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[2, 1, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[2, 2, 0], Is.EqualTo(0).Within( 1e-10));
            Assert.That(hessian[2, 2, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[2, 2, 2], Is.EqualTo(0).Within(1e-10));

        }

        static Heights Sphere()
        {
            Heights sphere = new Heights();
            sphere.SetX(new Point(1));

            IRootFinder rooter = new NewtonMethod(1e-10);

            double r = 0.5;
            double c = 0.5;
            Tensor2 C = Tensor2.Unit(3);
            IScalarFunction alpha = new QuadraticPolynomial(-r * r + c * c, Tensor1.Vector(-2 * c, 0, 0), C);

            IHeightFunctionY circle = new ZeroLine(rooter, alpha, Tensor1.Vector(0, 1, 0), Tensor1.Vector(0, 0, 0));
            sphere.SetY(circle);

            IHeightFunctionZ surface = new ZeroPlane(rooter, alpha, Tensor1.Vector(0, 0, 1), Tensor1.Vector(0, 0, 0));
            sphere.SetZ(surface);
            return sphere;
        }

        [Test]
        public static void HalfRadius()
        {
            NestedMapping m = NestedMapping.Dimension1(Sphere(), Plane(0));

            double x = 0.3;
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Vector(x));

            double Tx = 0.5 * x + 0.5;
            Assert.That(evaluation[0], Is.EqualTo(Tx).Within(1e-10));

            double dxTx = 0.5;
            Assert.That(jacobian[0, 0], Is.EqualTo(dxTx).Within(1e-10));
            Assert.That(hessian[0, 0, 0], Is.EqualTo(0).Within(1e-10));
        }

        [Test]
        public static void HalfCircle()
        {
            NestedMapping m = NestedMapping.Dimension2(Sphere(), Plane(0));
            double x = -0.2;
            double y = -0.2;
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Vector(x, y));

            double Tx = 0.5 * x + 0.5;
            double Ty = Math.Sqrt(Tx - Tx * Tx) * 0.5 * (y + 1);
            Assert.That(evaluation[0], Is.EqualTo(Tx).Within(1e-10));
            Assert.That(evaluation[1], Is.EqualTo(Ty).Within(1e-10));

            double dxTx = 0.5;
            double dxTy = -0.125 * x * (y + 1) / Math.Sqrt(0.25 - 0.25 * x * x);
            double dyTy = 0.5 * Math.Sqrt(0.25 - 0.25 * x * x);
            Assert.That(jacobian[0, 0], Is.EqualTo(dxTx).Within(1e-10));
            Assert.That(jacobian[0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[1, 0], Is.EqualTo(dxTy).Within(1e-10));
            Assert.That(jacobian[1, 1], Is.EqualTo(dyTy).Within(1e-10));

            double dxxTy = (0.125 * y + 0.125) / ((x * x - 1) * Math.Sqrt(0.25 - 0.25 * x * x));
            double dxyTy = -0.125 * x / Math.Sqrt(0.25 - 0.25 * x * x);

            Assert.That(hessian[0, 0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0, 0], Is.EqualTo(dxxTy).Within(1e-10));
            Assert.That(hessian[1, 0, 1], Is.EqualTo(dxyTy).Within(1e-10));
            Assert.That(hessian[1, 1, 0], Is.EqualTo(dxyTy).Within(1e-10));
            Assert.That(hessian[1, 1, 1], Is.EqualTo(0).Within(1e-10));
        }

        [Test]
        public static void QuarterSphere()
        {
            NestedMapping m = NestedMapping.Dimension3(Sphere(), Plane(0));
            double x = 0.2;
            double y = 0.2;
            double z = 0.2;
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Vector(x, y, z));

            double Tx = 0.5 * x + 0.5;
            double Ty = Math.Sqrt(Tx - Tx * Tx) * 0.5 * (y + 1);
            double Tz = Math.Sqrt(Tx - Tx * Tx - Ty * Ty) * 0.5 * (z + 1);
            Assert.That(evaluation[0], Is.EqualTo(Tx).Within(1e-10));
            Assert.That(evaluation[1], Is.EqualTo(Ty).Within( 1e-10));
            Assert.That(evaluation[2], Is.EqualTo(Tz).Within(1e-10));

            double dxTx = 0.5;
            double dxTy = -0.125 * x * (y + 1) / Math.Sqrt(0.25 - 0.25 * x * x);
            double dyTy = 0.5 * Math.Sqrt(0.25 - 0.25 * x * x);
            double dxTz = 0.125 * x * (y * y + 2 * y - 3) * (z + 1) / Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3));
            double dyTz = 0.125 * (x * x - 1) * (y + 1) * (z + 1) / Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3));
            double dzTz = 0.5 * Math.Sqrt(-0.25 * (y + 1) * (y + 1) * (0.25 - 0.25 * x * x) - 0.25 * x * x + 0.25);

            Assert.That(jacobian[0, 0], Is.EqualTo(dxTx).Within(1e-10));
            Assert.That(jacobian[0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[0, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[1, 0], Is.EqualTo(dxTy).Within(1e-10));
            Assert.That(jacobian[1, 1], Is.EqualTo(dyTy).Within(1e-10));
            Assert.That(jacobian[1, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(jacobian[2, 0], Is.EqualTo(dxTz).Within(1e-10));
            Assert.That(jacobian[2, 1], Is.EqualTo(dyTz).Within(1e-10));
            Assert.That(jacobian[2, 2], Is.EqualTo(dzTz).Within(1e-10));


            double dxxTy = (0.125 * y + 0.125) / ((x * x - 1) * Math.Sqrt(0.25 - 0.25 * x * x));
            double dxyTy = -0.125 * x / Math.Sqrt(0.25 - 0.25 * x * x);

            Assert.That(hessian[0, 0, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 0, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 0, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 1, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 2, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 2, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[0, 2, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 0, 0], Is.EqualTo(dxxTy).Within(1e-10));
            Assert.That(hessian[1, 0, 1], Is.EqualTo(dxyTy).Within(1e-10));
            Assert.That(hessian[1, 0, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 0], Is.EqualTo(dxyTy).Within(1e-10));
            Assert.That(hessian[1, 1, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 1, 2], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 2, 0], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 2, 1], Is.EqualTo(0).Within(1e-10));
            Assert.That(hessian[1, 2, 2], Is.EqualTo(0).Within(1e-10));

            double dxxTz = 0.125 * (y * y + 2 * y - 3) * (-z - 1) / ((x * x - 1) * Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3)));
            double dxyTz = 0.125 * x * (y + 1) * (z + 1) / Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3));
            double dxzTz = 0.125 * x * (y * y + 2 * y - 3) / Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3));
            double dyyTz = 0.5 * (x * x - 1) * (-z - 1) / ((y * y + 2 * y - 3) * Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3)));
            double dyzTz = -0.125 * (y + 1) * (0.25 - 0.25 * x * x) / Math.Sqrt(-0.25 * (y + 1) * (y + 1) * (0.25 - 0.25 * x * x) - 0.25 * x * x + 0.25);

            Assert.That(hessian[2, 0, 0], Is.EqualTo(dxxTz).Within(1e-10));
            Assert.That(hessian[2, 0, 1], Is.EqualTo(dxyTz).Within(1e-10));
            Assert.That(hessian[2, 0, 2], Is.EqualTo(dxzTz).Within(1e-10));
            Assert.That(hessian[2, 1, 0], Is.EqualTo(dxyTz).Within(1e-10));
            Assert.That(hessian[2, 1, 1], Is.EqualTo(dyyTz).Within(1e-10));
            Assert.That(hessian[2, 1, 2], Is.EqualTo(dyzTz).Within(1e-10));
            Assert.That(hessian[2, 2, 0], Is.EqualTo(dxzTz).Within(1e-10));
            Assert.That(hessian[2, 2, 1], Is.EqualTo(dyzTz).Within(1e-10));
            Assert.That(hessian[2, 2, 2], Is.EqualTo(0).Within( 1e-10));

        }
    }
}

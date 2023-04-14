using IntersectingQuadrature.Tensor;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Mapper {
    internal class NestedMappingTest {

        static Heights Plane(double m) {
            Heights plane = new Heights();
            plane.SetX(new Point(m));
            plane.SetY(new Line(m));
            plane.SetZ(new Plane(m));

            return plane;
        }

        [Test]
        public static void Line() {
            NestedMapping m = NestedMapping.Dimension1(Plane(1), Plane(0));
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Zeros(1));

            Assert.AreEqual(evaluation[0], 0.5, 1e-10);
            Assert.AreEqual(jacobian[0,0], 0.5, 1e-10);
            Assert.AreEqual(hessian[0,0,0], 0, 1e-10);
        }

        [Test]
        public static void Square() {
            NestedMapping m = NestedMapping.Dimension2(Plane(1), Plane(0));
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Zeros(2));
            
            Assert.AreEqual(evaluation[0], 0.5, 1e-10);
            Assert.AreEqual(evaluation[1], 0.5, 1e-10);

            Assert.AreEqual(jacobian[0, 0], 0.5, 1e-10);
            Assert.AreEqual(jacobian[0, 1], 0, 1e-10);
            Assert.AreEqual(jacobian[1, 0], 0, 1e-10);
            Assert.AreEqual(jacobian[1, 1], 0.5, 1e-10);

            Assert.AreEqual(hessian[0, 0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 1], 0, 1e-10);
        }

        [Test]
        public static void Cube() {
            NestedMapping m = NestedMapping.Dimension3(Plane(1), Plane(0));
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Zeros(3));

            Assert.AreEqual(evaluation[0], 0.5, 1e-10);
            Assert.AreEqual(evaluation[1], 0.5, 1e-10);
            Assert.AreEqual(evaluation[2], 0.5, 1e-10);

            Assert.AreEqual(jacobian[0, 0], 0.5, 1e-10);
            Assert.AreEqual(jacobian[0, 1], 0, 1e-10);
            Assert.AreEqual(jacobian[0, 2], 0, 1e-10);
            Assert.AreEqual(jacobian[1, 0], 0, 1e-10);
            Assert.AreEqual(jacobian[1, 1], 0.5, 1e-10);
            Assert.AreEqual(jacobian[1, 2], 0, 1e-10);
            Assert.AreEqual(jacobian[2, 0], 0, 1e-10);
            Assert.AreEqual(jacobian[2, 1], 0, 1e-10);
            Assert.AreEqual(jacobian[2, 2], 0.5, 1e-10);

            Assert.AreEqual(hessian[0, 0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 0, 2], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 2], 0, 1e-10);
            Assert.AreEqual(hessian[0, 2, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 2, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 2, 2], 0, 1e-10);

            Assert.AreEqual(hessian[1, 0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0, 2], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 2], 0, 1e-10);
            Assert.AreEqual(hessian[1, 2, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 2, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 2, 2], 0, 1e-10);

            Assert.AreEqual(hessian[2, 0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[2, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[2, 0, 2], 0, 1e-10);
            Assert.AreEqual(hessian[2, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[2, 1, 1], 0, 1e-10);
            Assert.AreEqual(hessian[2, 1, 2], 0, 1e-10);
            Assert.AreEqual(hessian[2, 2, 0], 0, 1e-10);
            Assert.AreEqual(hessian[2, 2, 1], 0, 1e-10);
            Assert.AreEqual(hessian[2, 2, 2], 0, 1e-10);

        }

        static Heights Sphere() {
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
        public static void HalfRadius() {
            NestedMapping m = NestedMapping.Dimension1(Sphere(), Plane(0));
            
            double x = 0.3;
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Vector(x));

            double Tx = 0.5 * x + 0.5;
            Assert.AreEqual(evaluation[0], Tx, 1e-10);
            
            double dxTx = 0.5;
            Assert.AreEqual(jacobian[0, 0], dxTx, 1e-10);
            Assert.AreEqual(hessian[0, 0, 0], 0, 1e-10);
        }

        [Test]
        public static void HalfCircle() {
            NestedMapping m = NestedMapping.Dimension2(Sphere(), Plane(0));
            double x = -0.2;
            double y = -0.2;
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Vector(x,y));

            double Tx = 0.5 * x + 0.5;
            double Ty = Math.Sqrt(Tx - Tx * Tx) * 0.5 * (y + 1);
            Assert.AreEqual(evaluation[0], Tx, 1e-10);
            Assert.AreEqual(evaluation[1], Ty, 1e-10);

            double dxTx = 0.5;
            double dxTy = -0.125 * x * (y+1) / Math.Sqrt(0.25 -0.25*x*x);
            double dyTy = 0.5 * Math.Sqrt(0.25 - 0.25 * x * x);
            Assert.AreEqual(jacobian[0, 0], dxTx, 1e-10);
            Assert.AreEqual(jacobian[0, 1], 0, 1e-10);
            Assert.AreEqual(jacobian[1, 0], dxTy, 1e-10);
            Assert.AreEqual(jacobian[1, 1], dyTy, 1e-10);

            double dxxTy = (0.125 * y + 0.125) / ((x * x -1)*Math.Sqrt(0.25-0.25*x*x));
            double dxyTy = -0.125 * x / Math.Sqrt(0.25 - 0.25 * x * x);

            Assert.AreEqual(hessian[0, 0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0, 0], dxxTy, 1e-10);
            Assert.AreEqual(hessian[1, 0, 1], dxyTy, 1e-10);
            Assert.AreEqual(hessian[1, 1, 0], dxyTy, 1e-10);
            Assert.AreEqual(hessian[1, 1, 1], 0, 1e-10);
        }

        [Test]
        public static void QuarterSphere() {
            NestedMapping m = NestedMapping.Dimension3(Sphere(), Plane(0));
            double x = 0.2;
            double y = 0.2;
            double z = 0.2;
            (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) = m.EvaluateAndJacobianAndHessian(Tensor1.Vector(x,y,z));

            double Tx = 0.5 * x + 0.5;
            double Ty = Math.Sqrt(Tx - Tx * Tx) * 0.5 * (y + 1);
            double Tz = Math.Sqrt(Tx - Tx * Tx - Ty *Ty) * 0.5 * (z + 1);
            Assert.AreEqual(evaluation[0], Tx, 1e-10);
            Assert.AreEqual(evaluation[1], Ty, 1e-10);
            Assert.AreEqual(evaluation[2], Tz, 1e-10);

            double dxTx = 0.5;
            double dxTy = -0.125 * x * (y + 1) / Math.Sqrt(0.25 - 0.25 * x * x);
            double dyTy = 0.5 * Math.Sqrt(0.25 - 0.25 * x * x);
            double dxTz = 0.125 * x *(y * y + 2 * y -3)*(z+1)/Math.Sqrt((x*x -1)*(y*y +2 *y -3));
            double dyTz = 0.125 * (x * x - 1) * (y + 1) * (z + 1) / Math.Sqrt((x * x - 1) *(y * y + 2 * y - 3)) ;
            double dzTz = 0.5 * Math.Sqrt(-0.25 * (y + 1) * (y + 1) * (0.25 - 0.25 * x * x) - 0.25 * x * x + 0.25);

            Assert.AreEqual(jacobian[0, 0], dxTx, 1e-10);
            Assert.AreEqual(jacobian[0, 1], 0, 1e-10);
            Assert.AreEqual(jacobian[0, 2], 0, 1e-10);
            Assert.AreEqual(jacobian[1, 0], dxTy, 1e-10);
            Assert.AreEqual(jacobian[1, 1], dyTy, 1e-10);
            Assert.AreEqual(jacobian[1, 2], 0, 1e-10);
            Assert.AreEqual(jacobian[2, 0], dxTz, 1e-10);
            Assert.AreEqual(jacobian[2, 1], dyTz, 1e-10);
            Assert.AreEqual(jacobian[2, 2], dzTz, 1e-10);


            double dxxTy = (0.125 * y + 0.125) / ((x * x - 1) * Math.Sqrt(0.25 - 0.25 * x * x));
            double dxyTy = -0.125 * x / Math.Sqrt(0.25 - 0.25 * x * x);

            Assert.AreEqual(hessian[0, 0, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 0, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 0, 2], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 1, 2], 0, 1e-10);
            Assert.AreEqual(hessian[0, 2, 0], 0, 1e-10);
            Assert.AreEqual(hessian[0, 2, 1], 0, 1e-10);
            Assert.AreEqual(hessian[0, 2, 2], 0, 1e-10);
            Assert.AreEqual(hessian[1, 0, 0], dxxTy, 1e-10);
            Assert.AreEqual(hessian[1, 0, 1], dxyTy, 1e-10);
            Assert.AreEqual(hessian[1, 0, 2], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 0], dxyTy, 1e-10);
            Assert.AreEqual(hessian[1, 1, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 1, 2], 0, 1e-10);
            Assert.AreEqual(hessian[1, 2, 0], 0, 1e-10);
            Assert.AreEqual(hessian[1, 2, 1], 0, 1e-10);
            Assert.AreEqual(hessian[1, 2, 2], 0, 1e-10);

            double dxxTz = 0.125 * (y * y + 2*y -3 ) * (-z -1) / ((x*x-1)*Math.Sqrt((x * x - 1)* (y * y + 2 * y - 3)));
            double dxyTz = 0.125 * x *(y+1)*(z+1) / Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3));
            double dxzTz = 0.125 * x * (y * y + 2 * y - 3) / Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3));
            double dyyTz = 0.5 *(x*x-1)*(-z-1) / ((y * y + 2 * y - 3) * Math.Sqrt((x * x - 1) * (y * y + 2 * y - 3)));
            double dyzTz = -0.125 * (y+1)*(0.25 -0.25 * x*x) / Math.Sqrt(-0.25 *(y+1)* (y + 1) *(0.25 -0.25*x*x) - 0.25*x*x +0.25);

            Assert.AreEqual(hessian[2, 0, 0], dxxTz, 1e-10);
            Assert.AreEqual(hessian[2, 0, 1], dxyTz, 1e-10);
            Assert.AreEqual(hessian[2, 0, 2], dxzTz, 1e-10);
            Assert.AreEqual(hessian[2, 1, 0], dxyTz, 1e-10);
            Assert.AreEqual(hessian[2, 1, 1], dyyTz, 1e-10);
            Assert.AreEqual(hessian[2, 1, 2], dyzTz, 1e-10);
            Assert.AreEqual(hessian[2, 2, 0], dxzTz, 1e-10);
            Assert.AreEqual(hessian[2, 2, 1], dyzTz, 1e-10);
            Assert.AreEqual(hessian[2, 2, 2], 0, 1e-10);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Experiments.Shapes;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example.Experiments
{
    internal static class SingleCell {

        public static void Line() {
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1), 1.5);
            IScalarFunction beta = new Sphere(Tensor1.Vector(1), 1.5);

            IQuadrater ruler = IntersectingQuadrature.Methods.Create();
            HyperRectangle cube = HyperRectangle.UnitCube(1);
            QuadratureRule rule = ruler.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, cube, 3, 0);

            IO.Write("nodesLine.txt", rule);
        }

        public static void Circle() {
            IScalarFunction alpha = new Sphere(Tensor1.Vector(0,0), 0.5);

            IQuadrater ruler = IntersectingQuadrature.Methods.Create();
            HyperRectangle cube = HyperRectangle.UnitCube(2);
            QuadratureRule rule = ruler.FindRule(alpha, Symbol.Minus, cube, 3, 0);

            IO.Write("nodesCircle.txt", rule);
        }

        public static void Sphere() {
            IScalarFunction alpha = new Sphere(Tensor1.Vector(0, 0, 0), 0.5);

            IQuadrater ruler = IntersectingQuadrature.Methods.Create();
            HyperRectangle cube = HyperRectangle.UnitCube(3);
            QuadratureRule rule = ruler.FindRule(alpha, Symbol.Zero, cube, 3, 0);

            IO.Write("nodesSphere.txt", rule);
        }

        public static void TorusCap() {
            IScalarFunction torus = new Torus(Tensor1.Vector(0.01, -2.01, -1.999), 2, 1);

            IQuadrater ruler = IntersectingQuadrature.Methods.Create();
            HyperRectangle cube = HyperRectangle.UnitCube(3);
            QuadratureRule rule = ruler.FindRule(torus, Symbol.Zero, cube, 3, 0);

            IO.Write("nodesTorus.txt", rule);
        }

        public static void CylindricSheet() {

            Tensor2 A = Tensor2.Zeros(3);
            A[0, 0] = 1;
            A[1, 1] = 1;
            double r = 3;
            double c = 3.9;
            IScalarFunction a = new QuadraticPolynomial(-r * r + c * c, Tensor1.Vector(2 * c , 0, 0), A) ;
            IScalarFunction alpha = new Sheet(a, 0.2);

            IQuadrater ruler = IntersectingQuadrature.Methods.Create();
            HyperRectangle cube = HyperRectangle.UnitCube(3);
            QuadratureRule rule = ruler.FindRule(alpha, Symbol.Zero, cube, 3, 0);

            IO.Write("nodesSheet.txt", rule);
        }

        public static void SphericSheet() {

            double d = 0.1;
            double r = 2;
            double cx = 2.9;
            double cy = 0;
            IScalarFunction sphere = new Sphere(Tensor1.Vector(-cx, -cy, 0), r);

            IScalarFunction alpha = new Sheet(sphere, d);

            IQuadrater ruler = IntersectingQuadrature.Methods.Create();
            HyperRectangle cube = HyperRectangle.UnitCube(3);
            QuadratureRule rule = ruler.FindRule(sphere, Symbol.Zero, cube, 1, 0);

            IO.Write("nodesSheet.txt", rule);
        }

        public static void Cylinder() {

            IScalarFunction beta = Plane3D.XY(0.00);
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(1.8, 0.1, 0 ), 1);


            IQuadrater finder = IntersectingQuadrature.Methods.Create();
            HyperRectangle unitDomain = HyperRectangle.UnitCube(3);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, unitDomain, 2);

            IO.Write("nodes.txt", rule);
        }

        public static void QuarterTube() {
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(1, 1, 0), 1);


            IQuadrater finder = IntersectingQuadrature.Methods.Create();
            HyperRectangle unitDomain = HyperRectangle.UnitCube(3);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, unitDomain, 4);

            IO.Write("nodes.txt", rule);
        }

        public static void Lip(int n = 2) {
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(-1, 0.5, 0), 1);
            IScalarFunction beta = new Cylinder(Tensor1.Vector(-1, -0.5, 0), 1);

            IQuadrater finder = IntersectingQuadrature.Methods.Create();
            HyperRectangle unitDomain = HyperRectangle.UnitCube(3);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, unitDomain, n);

            IO.Write("nodes.txt", rule);
        }

        public static void SkewPlane() {

            IScalarFunction alpha = new Plane(Tensor1.Vector(0.3, 1, 0.2), Tensor1.Zeros(3));

            IQuadrater finder = IntersectingQuadrature.Methods.Create();
            HyperRectangle cell = new HyperRectangle(3);
            cell.Center = Tensor1.Vector( -0.66666666666666674, 0, 0);
            cell.Diameters[0] = 0.66666666666666663;
            cell.Diameters[1] = 0.66666666666666663;
            cell.Diameters[2] = 0.66666666666666663;
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, cell, 2);

            IO.Write("nodes.txt", rule);
        }

        public static void Ufo() {
            double R = 0.9;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);

            IQuadrater finder = IntersectingQuadrature.Methods.Create();
            HyperRectangle cell = new HyperRectangle(3);
            double h = 2 / 8.0;
            cell.Center = Tensor1.Vector(-1 + h/ 2.0 + h* 0, -1 + h / 2.0 + h * 2 , -1 + h / 2.0 + h * 4);
            cell.Diameters[0] = h;
            cell.Diameters[1] = h;
            cell.Diameters[2] = h;
            QuadratureRule rule = finder.FindRule(beta, Symbol.Zero, alpha, Symbol.Minus, cell, 4);
            IO.Write("nodes.txt", rule);
        }
    }
}

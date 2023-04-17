using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example.Experiments {
    internal static class SingleCell {

        public static void Sphere() {
            IScalarFunction alpha = new Sphere(Tensor1.Vector(0, 0, 0), 0.5);

            Quadrater ruler = new Quadrater();
            HyperRectangle cube = new UnitCube(3);
            QuadratureRule rule = ruler.FindRule(alpha, Symbol.Zero, cube, 3, 0);

            IO.Write("nodesSphere.txt", rule);
        }

        public static void TorusCap() {
            IScalarFunction alpha = new Torus(Tensor1.Vector(0.01, -4.01, -2.9), 4, 2);

            Quadrater ruler = new Quadrater();
            HyperRectangle cube = new UnitCube(3);
            QuadratureRule rule = ruler.FindRule(alpha, Symbol.Zero, cube, 3, 0);

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

            Quadrater ruler = new Quadrater();
            HyperRectangle cube = new UnitCube(3);
            QuadratureRule rule = ruler.FindRule(alpha, Symbol.Zero, cube, 3, 0);

            IO.Write("nodesSheet.txt", rule);
        }

        public static void SphericSheet() {

            Tensor2 A = Tensor2.Zeros(3);
            A[0, 0] = 1;
            double d = 0.2;
            IScalarFunction a = new QuadraticPolynomial(-d * d, Tensor1.Zeros(3), A);

            double r = 3;
            double c = 3;
            Tensor2 B = Tensor2.Zeros(3);
            B[0, 0] = 2 * c;
            Tensor3 C = Tensor3.Zeros(3);
            C[0, 0, 0] = 1;
            C[1, 0, 1] = 1;
            C[2, 0, 2] = 1;
            IVectorFunction b = new QuadraticVectorPolynomial(Tensor1.Vector(-r * r + c * c, 0, 0), B, C);

            IScalarFunction alpha = new ScalarComposition(a, b);

            Quadrater ruler = new Quadrater();
            HyperRectangle cube = new UnitCube(3);
            QuadratureRule rule = ruler.FindRule(alpha, Symbol.Zero, cube, 3, 0);

            IO.Write("nodesSheet.txt", rule);
        }

        public static void Cylinder() {

            IScalarFunction beta = Plane3D.XY(0.00);
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(1.8, 0.1, 0 ), 1);


            Quadrater finder = new Quadrater();
            HyperRectangle unitDomain = new UnitCube(3);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, unitDomain, 2);

            IO.Write("nodes.txt", rule);
        }

        public static void QuarterTube() {
            IScalarFunction beta = Plane3D.XY(0);
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(1, 1, 0), 1);


            Quadrater finder = new Quadrater();
            HyperRectangle unitDomain = new UnitCube(3);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, unitDomain, 4);

            IO.Write("nodes.txt", rule);
        }

        public static void Lip(int n = 2) {
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(-1, 0.5, 0), 1);
            IScalarFunction beta = new Cylinder(Tensor1.Vector(-1, -0.5, 0), 1);

            Quadrater finder = new Quadrater();
            HyperRectangle unitDomain = new UnitCube(3);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, unitDomain, n);

            IO.Write("nodes.txt", rule);
        }

        public static void SkewPlane() {

            IScalarFunction alpha = new Plane(Tensor1.Vector(0.3, 1, 0.2), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = new HyperRectangle(3);
            cell.Center = Tensor1.Vector( -0.66666666666666674, 0, 0);
            cell.Diameters[0] = 0.66666666666666663;
            cell.Diameters[1] = 0.66666666666666663;
            cell.Diameters[2] = 0.66666666666666663;
            cell.ActiveDimensions.SetAll(true);
            cell.BodyDimension = 3;
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, cell, 2);

            IO.Write("nodes.txt", rule);
        }

        public static void Ufo() {
            double R = 0.9;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);

            Quadrater finder = new Quadrater();
            HyperRectangle cell = new HyperRectangle(3);
            double h = 2 / 8.0;
            cell.Center = Tensor1.Vector(-1 + h/ 2.0 + h* 0, -1 + h / 2.0 + h * 2 , -1 + h / 2.0 + h * 4);
            cell.Diameters[0] = h;
            cell.Diameters[1] = h;
            cell.Diameters[2] = h;
            cell.ActiveDimensions.SetAll(true);
            cell.BodyDimension = 3;
            QuadratureRule rule = finder.FindRule(beta, Symbol.Zero, alpha, Symbol.Minus, cell, 4);
            IO.Write("nodes.txt", rule);
        }

        public static void PlaneSurface(int n) {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1, 0, 0 ), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = new UnitCube(3);
            double scale = 0.1;
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Zero, cell, n);

            IScalarFunction f = new ConstantPolynomial(1);

            double exact = MathUtility.Pow(scale * 2.0, 2);
            double s = Quadrature.Evaluate(f, rule);
            double e = Math.Abs(s - exact);
            Console.WriteLine($"1,{e}");
            IO.Write("nodes.txt", rule);
        }

        public static void TwoPlaneSurface(int n) {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1, 0, 0), Tensor1.Zeros(3));
            IScalarFunction beta = new Plane(Tensor1.Vector(1, 0, 0.9), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = new UnitCube(3);
            double scale = 1;
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(beta, Symbol.Minus, alpha, Symbol.Zero, cell, n);

            IScalarFunction f = new ConstantPolynomial(1);

            double exact = MathUtility.Pow(scale * 2,2)/2;
            double s = Quadrature.Evaluate(f, rule);
            double e = Math.Abs(s - exact);
            Console.WriteLine($"TwoPlaneSurface Error: ,{e}");
            IO.Write("nodes.txt", rule);
        }

        public static void TwoPlaneVolume(int n) {

            double a = 0.5;
            IScalarFunction alpha = new Plane (Tensor1.Vector(1, 0, a), Tensor1.Zeros(3));
            IScalarFunction beta = new Plane(Tensor1.Vector(a, 0, 1), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = new UnitCube(3);
            double scale = 1;
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(beta, Symbol.Minus, alpha, Symbol.Minus, cell, n);

            IScalarFunction f = new ConstantPolynomial(1);

            double exact = MathUtility.Pow(scale * 2, 3) / 4 + scale * scale * 2 * (scale * a);
            double s = Quadrature.Evaluate(f, rule);
            double e = Math.Abs(s - exact);
            Console.WriteLine($"1,{e}");
            IO.Write("nodes.txt", rule);
        }
    }
}

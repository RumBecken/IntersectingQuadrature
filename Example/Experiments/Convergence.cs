using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.TensorAnalysis;

namespace Example.Experiments {
    

    internal static class Convergence {

        public static void SphereVolume(int n = 2) {
            double R = 0.7;
            IScalarFunction alpha = new Sphere(Tensor1.Vector(0, 0, 0.0), R);
            int cells = 5;
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 4.0 / 3.0 * Math.PI * MathUtility.Pow(R, 3);
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, n, cells);

                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                results.Add(new[] { i, s });
                cells *= 2;

                IO.Write($"nodes{i}.txt", rules);
            }
            IO.Write("convergence.txt", results);
        }

        public static void SineInSphere(int n = 2) {
            IScalarFunction alpha = new Sphere(Tensor1.Vector(0, 0, 0), 0.7);
            int cells = 5;
            IScalarFunction f = new SineSurface(1, 2);
            double exact = 0;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write("convergence.txt", results);
        }

        public static void SphereSurface(int n = 2) {
            double R = 0.7;
            IScalarFunction alpha = new Sphere(Tensor1.Vector(0, 0, 0), R);
            int cells = 5;
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 4.0 * Math.PI * MathUtility.Pow(R, 2);
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                results.Add(new[] { i, s });
                cells *= 2;

                IO.Write($"nodes{i}.txt", rules);
            }
            IO.Write("convergence.txt", results);
        }

        public static void LipVolume(int n = 2) {
            double r = 1.5;
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(-1, -1, 0), r);
            IScalarFunction beta = new Cylinder(Tensor1.Vector(-1, 1, 0), r);
            int cells = 4;
            IScalarFunction f = new ConstantPolynomial(1);

            //https://en.wikipedia.org/wiki/Circular_segment
            double h = r - 1;
            double exact = 2 * (r * r * Math.Acos(1 - h / r) - (r - h) * Math.Sqrt(r * r - MathUtility.Pow(r - h, 2)));
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells}, {s}");
                results.Add(new[] { i, s });
                cells *= 2;
                IO.Write($"nodes{i}.txt", rules);
            }
            IO.Write("convergence.txt", results);
        }

        public static void LipSurface(int n = 2) {
            double r = 1.5;
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(-1, -1, 0), r);
            IScalarFunction beta = new Cylinder(Tensor1.Vector(-1, 1, 0), r);
            int cells = 4;
            IScalarFunction f = new ConstantPolynomial(1);

            //https://en.wikipedia.org/wiki/Circular_segment
            double exact = 2 * r * Math.Acos(1.0 / r);
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Minus, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells}, {s}");
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write("convergence.txt", results);
        }

        public static void LipLine(int n = 2) {
            double r = 1.5;
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(-1, -1, 0), r);
            IScalarFunction beta = new Cylinder(Tensor1.Vector(-1, 1, 0), r);
            int cells = 4;
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Circular_segment
            double exact = 2;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells}, {s}");
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write("convergence.txt", results);
        }

        public static void HalfSphereVolume(int n = 2) {
            IScalarFunction alpha = new Plane(Tensor1.Vector(0.5, 1, 0.1), Tensor1.Zeros(3));
            int cells = 5;
            IScalarFunction beta = new Sphere(Tensor1.Zeros(3), 0.7);
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 2.0 / 3.0 * Math.PI * MathUtility.Pow(0.7, 3);
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 50; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells}, {s}");
                results.Add(new[] { i, s });
                cells += 1;
            }
            IO.Write("convergence.txt", results);
        }

        public static void HalfSphereSurface(int n = 2) {
            IScalarFunction alpha = new Plane(Tensor1.Vector(0.5, 1, 0.1), Tensor1.Zeros(3));
            int cells = 6;
            IScalarFunction beta = new Sphere(Tensor1.Zeros(3), 0.7);
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 2.0 * Math.PI * MathUtility.Pow(0.7, 2);
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells}, {s}");
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write("convergence.txt", results);
        }

        public static void HalfSphereLine(int n = 2) {
            IScalarFunction alpha = new Plane(Tensor1.Vector(0.5, 1, 0.1), Tensor1.Zeros(3));
            int cells = 6;
            IScalarFunction beta = new Sphere(Tensor1.Zeros(3), 0.7);
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 2.0 * Math.PI * MathUtility.Pow(0.7, 1);
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells}, {s}");
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write("convergence.txt", results);
        }

        public static void SineInUfoLine(int n = 2) {
            int cells = 6;
            IScalarFunction beta = new Sphere(Tensor1.Vector(0, 0, 0.51), 1);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(0, 0, -0.49), 1);
            IScalarFunction f = new SineSurface(1, 2);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = 0;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Zero, n, cells);

                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                IO.Write($"nodes{i}.txt", rules);
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write("convergence.txt", results);
        }

        public static void CylinderVolume(int n) {
            double r = 1.5;
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(-1.0, -1.0, 0), r);
            int cells = 4;
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = Math.PI * MathUtility.Pow(r, 2) * 2.0 / 4.0;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write("convergence.txt", results);
        }

        public static void CylinderSurface(int n) {
            double r = 1.5;
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(-1.0, -1.0, 0), r);
            int cells = 4;
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = Math.PI * r;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write("convergence.txt", results);
        }

        public static void Sine(int n = 2) {
            IScalarFunction sine = new SineSurface(0.2, 1.1);
            IScalarFunction alpha = sine;
            //IScalarFunction f = new Cosine(1, 2*Math.PI);
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 4.0;

            int cells = 8;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, n, cells);
                double s = Quadrature.Evaluate(f, rules);
                double e = Math.Abs(s - exact);
                Console.WriteLine($"{cells},{e}");
                results.Add(new[] { i, s });
                cells *= 2;
                IO.Write($"nodes{i}.txt", rules);
            }
            IO.Write("convergence.txt", results);
        }

        public static void Plane(int n = 2) {
            IScalarFunction alpha = new Plane(Tensor1.Vector(0, 0.3, 1), Tensor1.Zeros(3));
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 2 * 2 * Math.Sqrt(MathUtility.Pow(1, 2) + MathUtility.Pow(0.3, 2));

            int cells = 10;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                results.Add(new[] { i, s });
                cells *= 2;
                IO.Write($"nodes{i}.txt", rules);
            }
            IO.Write("convergence.txt", results);
        }

        public static void SinePlane(int n = 2) {
            IScalarFunction sine = new SineSurface(0.2, 1.1);
            IScalarFunction alpha = sine;
            IScalarFunction beta = new Plane(Tensor1.Vector(0, 0, 1), Tensor1.Zeros(3));
            IScalarFunction f = new ConstantPolynomial(1);

            int cells = 8;
            List<double[]> results = new List<double[]>();
            double exact = 2.0;
            for (int i = 0; i < 4; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, n, cells);
                double s = Quadrature.Evaluate(f, rules);
                double e = Math.Abs(s - exact);
                Console.WriteLine($"{cells},{e}");
                results.Add(new[] { i, s });
                cells *= 2;
                IO.Write($"nodes{i}.txt", rules);
            }
            IO.Write("convergence.txt", results);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.TensorAnalysis;

namespace Example.Experiments {
    internal class Ufo {
        public static void VolumeH(int n = 2) {
            double R = 0.9;
            double h = R - 0.5;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = Math.PI * MathUtility.Pow(h, 2) / 3 * (3 * R - h) * 2 / 4;
            List<double[]> results = new List<double[]>();
            for (int i = 8; i < 65; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, n, i);
                //IO.Write($"nodesVolume{n}_{i}.txt", rules);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{i}, {s}");
                long l = 0;
                foreach (QuadratureRule rule in rules) {
                    l += rule.Count;
                }

                results.Add(new[] { i, s, l });
            }
            IO.Write($"volumeConvergenceH{n}.txt", results);
        }

        public static void VolumeAdaptive(int n = 2) {
            double R = 0.9;
            double h = R - 0.5;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = Math.PI * MathUtility.Pow(h, 2) / 3 * (3 * R - h) * 2 / 4;
            List<double[]> results = new List<double[]>();
            for (int i = 8; i < 65; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, n, i);
                //IO.Write($"nodesVolume{n}_{i}.txt", rules);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                long l = 0;
                foreach (QuadratureRule rule in rules) {
                    l += rule.Count;
                }
                Console.WriteLine($"{i}, {s}, {l}");
                results.Add(new[] { i, s, l });
            }
            IO.Write($"volumeConvergenceAdaptive{n}.txt", results);
        }

        public static void Volume(int n = 2) {
            double R = 0.9;
            double h = R - 0.5;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = Math.PI * MathUtility.Pow(h, 2) / 3 * (3 * R - h) * 2 / 4;
            List<double[]> results = new List<double[]>();
            int cells = 10;
            for (int i = 0; i < 5; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, n, cells, i);
                //IO.Write($"nodesVolume{n}_{i}.txt", rules);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{i}, {s}");
                results.Add(new[] { MathUtility.Pow(2, i), s });
            }
            IO.Write($"volumeConvergence{n}.txt", results);
        }

        public static void SurfaceH(int n = 2) {
            double R = 0.9;
            double h = R - 0.5;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = 2.0 * Math.PI * R * h / 4.0;
            List<double[]> results = new List<double[]>();
            for (int i = 8; i < 65; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Minus,  n, i);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                //IO.Write($"nodesSurfaceH{n}_{i}.txt", rules);
                Console.WriteLine($"{i},{s}");
                long l = 0;
                foreach (QuadratureRule rule in rules) {
                    l += rule.Count;
                }

                results.Add(new[] { i, s, l });
            }
            IO.Write($"surfaceConvergenceH{n}.txt", results);
        }

        public static void SurfaceAdaptive(int n = 2) {
            double R = 0.9;
            double h = R - 0.5;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = 2.0 * Math.PI * R * h / 4.0;
            List<double[]> results = new List<double[]>();
            for (int i = 8; i < 65; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Minus, n, i);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                //IO.Write($"nodesSurfaceH{n}_{i}.txt", rules);
                Console.WriteLine($"{i},{s}");
                long l = 0;
                foreach (QuadratureRule rule in rules) {
                    l += rule.Count;
                }

                results.Add(new[] { i, s, l });
            }
            IO.Write($"surfaceConvergenceAdaptive{n}.txt", results);
        }

        public static void Surface(int n = 2) {
            double R = 0.9;
            double h = R - 0.5;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = 2.0 * Math.PI * R * h / 4.0;
            int cells = 10;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 5; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(beta, Symbol.Zero, alpha, Symbol.Minus, n, cells, i);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                //IO.Write($"nodesSurface{n}_{i}.txt", rules);
                Console.WriteLine($"{i},{s}");
                results.Add(new[] { MathUtility.Pow(2, i), s });
            }
            IO.Write($"surfaceConvergence{n}.txt", results);
        }

        public static void LineH(int n = 2) {
            double R = 0.9;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = 2.0 * Math.PI * Math.Sqrt(R * R - 0.5 * 0.5) / 4.0;
            List<double[]> results = new List<double[]>();
            for (int i = 8; i < 65; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Zero, n, i);

                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{i},{s}");
                IO.Write($"nodesLineH{n}_{i}.txt", rules);
                long l = 0;
                foreach(QuadratureRule rule in rules) {
                    l += rule.Count;
                }

                results.Add(new[] { i, s, l });
            }
            IO.Write($"lineConvergenceH{n}.txt", results);
        }

        public static void LineAdaptive(int n = 2) {
            double R = 0.9;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = 2.0 * Math.PI * Math.Sqrt(R * R - 0.5 * 0.5) / 4.0;
            List<double[]> results = new List<double[]>();
            for (int i = 8; i < 65; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Zero, n, i);

                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{i},{s}");
                IO.Write($"nodesLineAdaptive{n}_{i}.txt", rules);
                long l = 0;
                foreach (QuadratureRule rule in rules) {
                    l += rule.Count;
                }
                results.Add(new[] { i, s, l });
            }
            IO.Write($"lineConvergenceAdaptive{n}.txt", results);
        }

        public static void Line(int n = 2) {
            double R = 0.9;
            IScalarFunction beta = new Sphere(Tensor1.Vector(-1, -1, 0.51), R);
            IScalarFunction alpha = new Sphere(Tensor1.Vector(-1, -1, -0.49), R);
            IScalarFunction f = new ConstantPolynomial(1);
            //https://en.wikipedia.org/wiki/Spherical_cap
            double exact = 2.0 * Math.PI * Math.Sqrt(R * R - 0.5 * 0.5) / 4.0;
            List<double[]> results = new List<double[]>();
            int cells = 10;
            for (int i = 0; i < 5; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Zero, n, cells, i);

                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{i},{s}");
                IO.Write($"nodesLine{n}_{i}.txt", rules);
                results.Add(new[] { MathUtility.Pow(2, i), s});
            }
            IO.Write($"lineConvergence{n}.txt", results);
        }
    }
}

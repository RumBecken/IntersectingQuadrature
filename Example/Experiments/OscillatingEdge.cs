using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example.Experiments {
    internal class OscillatingEdge {
        public static void Line(int n) {
            int cells = 8;
            IScalarFunction sine = new SineSurface(0.2, 1.1);
            IScalarFunction alpha = sine;
            IScalarFunction beta = new ScalarComposition(sine, new Rotation(Tensor1.Vector(0, 1, 0), Tensor1.Vector(0, 0, -1)));
            //IScalarFunction f = new Cosine(1, 2 * Math.PI);
            IScalarFunction f = new ConstantPolynomial(1);

            double exact = 2.9018098242473137628716230441128;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 5; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                results.Add(new[] { i, s });
                IO.Write($"nodesLine{n}_{cells}.txt", rules);
                cells *= 2;
            }
            IO.Write($"lineConvergence{n}.txt", results);
        }

        public static void Surface(int n) {
            int cells = 8;
            IScalarFunction sine = new SineSurface(0.2, 1.1);
            IScalarFunction alpha = sine;
            IScalarFunction beta = new ScalarComposition(sine, new Rotation(Tensor1.Vector(0, 1, 0), Tensor1.Vector(0, 0, -1)));
            //IScalarFunction f = new Cosine(1, 2 * Math.PI) ;
            IScalarFunction f = new ConstantPolynomial(1);

            double exact = 2.5048230500093248969863804012397;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 5; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Zero, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                IO.Write($"nodesSurface{n}_{cells}.txt", rules);
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write($"surfaceConvergence{n}.txt", results);
        }

        public static void Volume(int n) {
            int cells = 8;
            IScalarFunction sine = new SineSurface(0.2, 1.1);
            IScalarFunction alpha = sine;
            IScalarFunction beta = new ScalarComposition(sine, new Rotation(Tensor1.Vector(0, 1, 0), Tensor1.Vector(0, 0, -1)));
            //IScalarFunction f = new Cosine(1, 2*Math.PI);
            IScalarFunction f = new ConstantPolynomial(1);

            double exact = 2.0431849934260147426243415665995;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 5; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, n, cells);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                Console.WriteLine($"{cells},{s}");
                IO.Write($"nodesVolume{n}_{cells}.txt", rules);
                results.Add(new[] { i, s });
                cells *= 2;
            }
            IO.Write($"volumeConvergence{n}.txt", results);
        }
    }
}

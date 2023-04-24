using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;
using Example.Experiments.Shapes;


namespace Example.Experiments
{
    internal class ToricSection {

        public static void Line(int n = 2) {
            double torusR = 0.6;
            double tubeR = 0.3;
            IScalarFunction alpha = new Torus(Tensor1.Vector(0, 0, 0), torusR, tubeR);
            IScalarFunction section = new TensorSineSurface(0.2, 1.1);
            IScalarFunction beta = new ScalarComposition(section, new Rotation(Tensor1.Vector(0, 0, 1), Tensor1.Vector(0, 0.5, 1)));
            IScalarFunction sine = new TensorSine(0.2, 1.1);
            IScalarFunction f = new ScalarComposition(sine, new Rotation(Tensor1.Vector(0, 0, 1), Tensor1.Vector(0, 0.5, 1)));
            int cells = 30;
            double exact = 0;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 5; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Zero, beta, Symbol.Zero, n, cells, i);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                IO.Write($"nodesLine{n}_{i}.txt", rules);
                Console.WriteLine($"{i},{s}");
                results.Add(new[] { Algebra.Pow(2, i), s });
            }
            IO.Write($"lineConvergence{n}.txt", results);
        }

        public static void Surface(int n = 2) {
            double torusR = 0.6;
            double tubeR = 0.3;
            IScalarFunction alpha = new Torus(Tensor1.Vector(0, 0, 0), torusR, tubeR);
            IScalarFunction section = new TensorSineSurface(0.2, 1.1);
            IScalarFunction beta = new ScalarComposition(section, new Rotation(Tensor1.Vector(0, 0, 1), Tensor1.Vector(0, 0.5, 1)));
            IScalarFunction sine = new TensorSine(0.2, 1.1);
            IScalarFunction f = new ScalarComposition(sine, new Rotation(Tensor1.Vector(0, 0, 1), Tensor1.Vector(0, 0.5, 1)));
            int cells = 10;
            double exact = 0;
            List<double[]> results = new List<double[]>();
            for (int i = 0; i < 5; ++i) {
                QuadratureRule[,,] rules = Grid.FindRule(beta, Symbol.Zero, alpha, Symbol.Minus, n, cells, i);
                double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
                IO.Write($"nodesSurface{n}_{i}.txt", rules);
                Console.WriteLine($"{i},{s}");
                results.Add(new[] { Algebra.Pow(2, i), s });
            }
            IO.Write($"surfaceConvergence{n}.txt", results);
        }
    }
}

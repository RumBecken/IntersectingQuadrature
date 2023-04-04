using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using TensorAnalysis;

namespace Example.Experiments {
    internal class SlantedSheet {
        public static void Subdivision() {
            int n = 1;
            double gap = 0.01;
            double slope = 0.8;
            IScalarFunction alpha = new ParallelLines(gap, slope);
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 2 * 2 * gap * 2;

            int cells = 1;
            QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Minus, n, cells);
            double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
            Console.WriteLine($"{rules[0,0,0].Count},{s}");
            IO.Write($"nodes.txt", rules);
        }

        public static void Exact() {
            int n =1;
            double gap = 0.01;
            double slope = 0.8;
            IScalarFunction alpha = new ParallelLines(gap, slope);
            IScalarFunction beta = new LinearPolynomial(0, Tensor1.Vector(slope, -1,0));
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 2 * 1 * gap * 2;

            int cells = 1;
            QuadratureRule[,,] rules = Grid.FindRule(beta, Symbol.Minus, alpha, Symbol.Minus, n, cells);
            double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
            Console.WriteLine($"{rules[0, 0, 0].Count},{s}");
            IO.Write($"nodes.txt", rules);
        }

        public static void SubdivisionPlot() {
            int n = 1;
            double gap = 0.1;
            double slope = 0.8;
            IScalarFunction alpha = new ParallelLines(gap, slope);
            IScalarFunction beta = new LinearPolynomial(0, Tensor1.Vector(0, 0, 1));
            IScalarFunction f = new ConstantPolynomial(1);
            double exact = 2 * 1 * gap * 2;

            int cells = 1;
            QuadratureRule[,,] rules = Grid.FindRule(alpha, Symbol.Plus, beta, Symbol.Zero, n, cells);
            double s = Math.Abs(Quadrature.Evaluate(f, rules) - exact);
            Console.WriteLine($"{rules[0, 0, 0].Count},{s}");
            IO.Write($"nodes.txt", rules);
        }
    }
}

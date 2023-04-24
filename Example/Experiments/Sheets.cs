using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Experiments.Shapes;


namespace Example.Experiments
{
    internal partial class Sheets {

        public static void WaveSurface() {
            int n = 1;
            IScalarFunction potato = new Potato(0.01);
            IScalarFunction alpha = new Sheet(potato, 0.1);
            QuadratureRule[,,] rule = Grid.FindRule(alpha, Symbol.Zero, n, 20);

            IO.Write("nodes.txt", rule);
            //double s = Math.Abs(Quadrature.Evaluate(f, rule));
        }

        public static void TorusSurface() {
            IScalarFunction torus = new Torus(Tensor1.Vector(0.00, 0, 0), 0.7, 0.2);
            IScalarFunction alpha = new Sheet(torus, 0.04);

            Quadrater ruler = new Quadrater();
            HyperRectangle cube = new UnitHyperCube(3);
            QuadratureRule[,,] rule = Grid.FindRule(alpha, Symbol.Zero, 1, 12);

            IO.Write("nodesTorus.txt", rule);
        }

        public static void SphereSurface() {
            IScalarFunction sphere = new Sphere(Tensor1.Vector(0.00, 0, 0), 0.7);
            IScalarFunction alpha = new Sheet(sphere, 0.05);
            QuadratureRule[,,] rule = Grid.FindRule(sphere, Symbol.Minus, alpha, Symbol.Zero, 2, 10);
            QuadratureRule[,,] rule1 = Grid.FindRule(sphere, Symbol.Plus, alpha, Symbol.Zero, 2, 10);
            IO.Write("nodesSphere.txt", rule, rule1);
        }
    }
}

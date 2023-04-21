using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Experiments
{
    internal partial class Sheets {

        public static void WaveSurface() {
            int n = 1;
            IScalarFunction potato = new Potato();
            IScalarFunction alpha = new Sheet(potato, 0.04);

            QuadratureRule[,,] rule = Grid.FindRule(alpha, Symbol.Zero, n, 30);

            IO.Write("nodes.txt", rule);
            //double s = Math.Abs(Quadrature.Evaluate(f, rule));
        }

        public static void TorusSurface() {
            IScalarFunction torus = new Torus(Tensor1.Vector(0.00, 0, 0), 0.7, 0.2);
            IScalarFunction alpha = new Sheet(torus, 0.01);

            Quadrater ruler = new Quadrater();
            HyperRectangle cube = new UnitHyperCube(3);
            QuadratureRule[,,] rule = Grid.FindRule(alpha, Symbol.Zero, 1, 12);

            IO.Write("nodesTorus.txt", rule);
        }

        public static void SphereSurface() {
            IScalarFunction sphere = new Sphere(Tensor1.Vector(0.00, 0, 0), 0.8);
            IScalarFunction alpha = new Sheet(sphere, 0.04);

            QuadratureRule[,,] rule = Grid.FindRule(sphere, Symbol.Zero, 1, 5);

            IO.Write("nodesSphere.txt", rule);
        }
    }
}

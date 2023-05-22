using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Experiments.Shapes;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example.Experiments
{
    internal static class Grid {

        public static void Volcano() {
            int n = 1;
            int cells =8;
            IScalarFunction volcano = new Volcano();

            //QuadratureRule[,,] rule = Grid.FindRule(volcano, Symbol.Zero, n, cells);
            QuadratureRule[,,] rule = Grid.FindRule(volcano, Symbol.Minus, new Sheet(volcano, 0.02), Symbol.Zero, n, cells);
            QuadratureRule[,,] rule1 = Grid.FindRule(volcano, Symbol.Plus, new Sheet(volcano, 0.02), Symbol.Zero, n, cells);

            IO.Write("nodesVolcano.txt", rule);
            IO.Write("nodesVolcano1.txt", rule1);
            //double s = Math.Abs(Quadrature.Evaluate(f, rule));
        }


        public static void Potato() {
            int n = 1;
            IScalarFunction potato = new Potato(0.5);

            QuadratureRule[,,] rule = Grid.FindRule(potato, Symbol.Zero, n, 10);

            IO.Write("nodes.txt", rule);
            //double s = Math.Abs(Quadrature.Evaluate(f, rule));
        }

        public static void Torus() {
            IScalarFunction alpha = new Torus(Tensor1.Vector(0.00, 0, 0), 0.7, 0.2);
            QuadratureRule[,,] rule = Grid.FindRule(alpha, Symbol.Zero, 1, 12);

            IO.Write("nodesTorus.txt", rule);
        }

        public static void WigglyCylinderSubdivision() {
            double gap = 0.001;
            IScalarFunction alpha = new WigglyCylinder(Tensor1.Vector(0, 0, 0), 2.0 / 3 + gap, 0.2);
            QuadratureRule[,,] rule = Grid.FindRule(alpha, Symbol.Zero, 2, 6);

            IO.Write("nodesWigglyCylinderSubdivision.txt", rule);
        }

        public static void WigglyCylinder() {
            double gap = 0.001;
            IScalarFunction alpha = new WigglyCylinder(Tensor1.Vector(0, 0, 0), 2.0 / 3 + gap, 0.2);
            IScalarFunction beta = new GradientComponent(alpha, 1);
            QuadratureRule[,,] rule = Grid.FindRule(beta, Symbol.Minus, alpha, Symbol.Zero, 2, 6);
            QuadratureRule[,,] rule1 = Grid.FindRule(beta, Symbol.Plus, alpha, Symbol.Zero, 2, 6);
            IO.Write("nodesWigglyCylinder.txt", rule, rule1);
        }

        public static void Cylinder(int cells = 5) {
            IScalarFunction beta = Plane3D.XY(0.2);
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(-1,-1,0), 0.8);
            QuadratureRule[,,] rule = FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, 2, cells);
            IO.Write("nodes.txt", rule);
        }

        public static void HalfSphere(int cells = 5) {
            //IScalarFunction beta = new Plane(new Vector() { X = 0.3, Y = 1, Z = 0.2 }, new Vector() { Y = 0.0 });
            IScalarFunction beta = new Plane(Tensor1.Vector(0, 1, 0.1 ), Tensor1.Zeros(3));
            //IScalarFunction beta = Plane.XY(0.0);
            
            IScalarFunction alpha = new Sphere(Tensor1.Zeros(3), 0.7);
            QuadratureRule[,,] rule = FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, 2, cells);
            IO.Write("nodes.txt", rule);

        }

        public static void Ufo(int cells = 5) {
            //IScalarFunction beta = new Plane(new Vector() { X = 0.3, Y = 1, Z = 0.2 }, new Vector() { Y = 0.0 });
            IScalarFunction beta = new Sphere(Tensor1.Vector(0,0,0.49), 1);
            //IScalarFunction beta = Plane.XY(0.0);

            IScalarFunction alpha = new Sphere(Tensor1.Vector(0, 0, -0.51), 1);
            QuadratureRule[,,] rule = FindRule(alpha, Symbol.Minus, beta, Symbol.Zero, 2, cells);
            IO.Write("nodes.txt", rule);

        }

        public static void Sphere(int cells = 10) {

            IScalarFunction alpha = new Sphere(Tensor1.Vector(0,0,0.0153466), 0.7);
            QuadratureRule[,,] rule = FindRule(alpha, Symbol.Zero, 2, cells);
            IO.Write("nodes.txt", rule);

        }

        public static void Planes(int cells = 5) {
            IScalarFunction beta = new Plane(Tensor1.Vector(0.3, 1, 0.2 ), Tensor1.Zeros(3));
            IScalarFunction alpha = Plane3D.XY(2.0);

            QuadratureRule[,,] rule = FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, 2, cells);
            IO.Write("nodes.txt", rule);
        }

        public static void Plane(int cells = 2) {
            IScalarFunction alpha = Plane3D.XY(0.0);

            QuadratureRule[,,] rule = FindRule(alpha, Symbol.Zero, 2, cells);
            IO.Write("nodes.txt", rule);
        }

        public static QuadratureRule[,,] FindRule(IScalarFunction alpha, Symbol signAlpha, IScalarFunction beta, Symbol signBeta, int n, int cells, int subdivisions = 0) {
            Quadrater finder = new Quadrater();
            HyperRectangle[,,] grid = CreateGrid(cells);
            QuadratureRule[,,] rules = new QuadratureRule[cells, cells, cells];
            int c = 0;
            for(int i= 0; i < cells; ++i) {
                for(int j = 0; j < cells; ++j) {
                    for (int k = 0; k < cells; ++k) {
                        HyperRectangle cell = grid[i, j, k];
                        QuadratureRule rule;
                        Console.WriteLine($"Working on cell{i},{j},{k}");
                        rule = finder.FindRule(alpha, signAlpha, beta, signBeta, cell, n, subdivisions);
                        rules[i, j, k] = rule;
                        ++c;
                    }
                }
            }
            return rules;
        }

        public static QuadratureRule[,,] FindRuleAdaptive(IScalarFunction alpha, Symbol signAlpha, IScalarFunction beta, Symbol signBeta, int n, int cells, double tau) {
            AdaptiveQuadrater finder = new AdaptiveQuadrater(tau);
            HyperRectangle[,,] grid = CreateGrid(cells);
            QuadratureRule[,,] rules = new QuadratureRule[cells, cells, cells];
            for (int i = 0; i < cells; ++i) {
                for (int j = 0; j < cells; ++j) {
                    for (int k = 0; k < cells; ++k) {
                        HyperRectangle cell = grid[i, j, k];
                        QuadratureRule rule;
                        Console.WriteLine($"Working on cell{i},{j},{k}");
                        rule = finder.FindRule(alpha, signAlpha, beta, signBeta, cell, n);
                        rules[i, j, k] = rule;
                    }
                }
            }
            return rules;
        }

        public static QuadratureRule[,,] FindRule(IScalarFunction alpha, Symbol signAlpha, int n, int cells) {
            Quadrater finder = new Quadrater();
            HyperRectangle[,,] grid = CreateGrid(cells);
            QuadratureRule[,,] rules = new QuadratureRule[cells, cells, cells];
            for (int i = 0; i < cells; ++i) {
                for (int j = 0; j < cells; ++j) {
                    for (int k = 0; k < cells; ++k) {
                        HyperRectangle cell = grid[i, j, k];
                        Console.WriteLine($"Working on cell{i},{j},{k}");
                        QuadratureRule rule = finder.FindRule(alpha, signAlpha, cell, n);
                        rules[i, j, k] = rule;
                    }
                }
            }
            return rules;
        }

        static HyperRectangle[,,] CreateGrid(int cells) {
            HyperRectangle[,,] grid = new HyperRectangle[cells, cells, cells];
            double h = 2.0 / cells;
            for (int i = 0; i < cells; ++i) {
                double xMinus = -1 + i * h;
                double xPlus = xMinus + h;
                for (int j = 0; j < cells; ++j) {
                    double yMinus = -1 + j * h;
                    double yPlus = yMinus + h;
                    for (int k = 0; k < cells; ++k) {
                        double zMinus = -1 + k * h;
                        double zPlus = zMinus + h;
                        HyperRectangle cell = Cell(xMinus, xPlus, yMinus, yPlus, zMinus, zPlus);
                        grid[i, j, k] = cell;
                    }
                }
            }
            return grid;
        }

        static HyperRectangle Cell(double xMinus, double xPlus, double yMinus, double yPlus, double zMinus, double zPlus) {
            HyperRectangle cell = new HyperRectangle(3);
            cell.Center = Tensor1.Vector(
                (xMinus + xPlus) / 2,
                (yMinus + yPlus) / 2,
                (zMinus + zPlus) / 2
            );
            cell.Diameters[0] = (-xMinus + xPlus);
            cell.Diameters[1] = (-yMinus + yPlus);
            cell.Diameters[2] = (-zMinus + zPlus);
            cell.ActiveDimensions.SetAll(true);
            cell.BodyDimension = 3;
            return cell;
        }
    }
}

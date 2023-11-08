using IntersectingQuadrature.Tensor;
using System;
using System.Linq;

namespace IntersectingQuadrature.Rules
{
    internal static class QuadratureRules
    {
        delegate QuadratureRule RuleFactory();

        public static QuadratureRule Gauss(int n, int dimension)
        {
            return Create(() => Rules.Gauss.Rule(n), dimension);
        }

        public static QuadratureRule GaussSubdivided(int n, int subdivisions, int dimension)
        {
            RuleFactory oneD = () => Rules.Gauss.Rule(n);
            return Create(() => Subdivide(oneD, subdivisions), dimension);
        }

        public static QuadratureRule GaussTschebychow(int n, int dimension) {
            return Create(() => Rules.GaussTschebychow.Rule(n), dimension);
        }

        public static QuadratureRule GaussLobatto(int n, int dimension)
        {
            return Create(() => Rules.GaussLobatto.Rule(n), dimension);
        }

        public static QuadratureRule GaussLobattoSubdivided(int n, int subdivisions, int dimension)
        {
            RuleFactory oneD = () => Rules.GaussLobatto.Rule(n);
            return Create(() => Subdivide(oneD, subdivisions), dimension);
        }

        public static QuadratureRule BruteForce(int n, int dimension)
        {
            return Create(()=> Rules.BruteForce.Rule(n), dimension);
        }

        public static QuadratureRule Plot(int n, int dimension)
        {
            return Create(() => Rules.Plot.Rule(n), dimension);
        }

        static QuadratureRule Subdivide(RuleFactory factory, int subdivisions)
        {
            QuadratureRule rule = factory();
            if (rule[0].Point.M == 0) {
                return rule;
            } else {
                return Subdivide(rule, subdivisions);
            }
        }

        static QuadratureRule Subdivide(QuadratureRule rule, int subdivisions) {
            QuadratureRule subRule = new QuadratureRule(rule.Count * Algebra.Pow(2, subdivisions));
            double h = 1 / Math.Pow(2, subdivisions);
            for (int i = 0; i < Math.Pow(2, subdivisions); ++i) {
                for (int j = 0; j < rule.Count; ++j) {
                    Tensor1 point = rule[j].Point * h + Tensor1.Vector(-1) + h * (2 * i + 1) * Tensor1.Vector(1);
                    double weight = rule[j].Weight * h;
                    QuadratureNode node = new QuadratureNode() {
                        Point = point,
                        Weight = weight
                    };
                    subRule.Add(node);
                }
            }
            return subRule;
        }

        static QuadratureRule Create(RuleFactory factory, int dimension)
        {
            if(dimension == 0) {
                QuadratureRule point = QuadratureRule.Allocate(1, dimension);
                point[0].Weight = 1;
                return point;
            } else {
                return Tensorize(factory, dimension);
            }
        }

        static QuadratureRule Tensorize(RuleFactory factory, int dimension) {
            QuadratureRule oneDimensional = factory();
            int count = Algebra.Pow(oneDimensional.Count, dimension);
            QuadratureRule rule = QuadratureRule.Allocate(count, dimension);
            foreach (QuadratureNode node in rule) {
                node.Weight = 1.0;
            }

            for (int d = 0; d < dimension; ++d) {
                int i = 0;
                foreach (QuadratureNode node in rule) {
                    int j = i / Algebra.Pow(oneDimensional.Count, d) % oneDimensional.Count;
                    node.Point[d] = oneDimensional[j].Point[0];
                    node.Weight *= oneDimensional[j].Weight;
                    ++i;
                }
            }
            return rule;
        }

    }
}

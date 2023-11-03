using IntersectingQuadrature.Tensor;
using System;
using System.Linq;

namespace IntersectingQuadrature.Rules
{
    internal static class QuadratureRules
    {
        public static QuadratureRule Gauss(int n, int dimension)
        {
            QuadratureRule oneD = Rules.Gauss.Rule(n);
            return Tensorize(oneD, dimension);
        }

        public static QuadratureRule GaussSubdivided(int n, int subdivisions, int dimension)
        {
            QuadratureRule oneD = Rules.Gauss.Rule(n);
            QuadratureRule subD = Subdivide(oneD, subdivisions);
            return Tensorize(subD, dimension);
        }

        public static QuadratureRule GaussTschebychow(int n, int dimension)
        {
            QuadratureRule oneD = Rules.GaussTschebychow.Rule(n);
            return Tensorize(oneD, dimension);
        }

        public static QuadratureRule GaussLobatto(int n, int dimension)
        {
            QuadratureRule oneD = Rules.GaussLobatto.Rule(n);
            return Tensorize(oneD, dimension);
        }

        public static QuadratureRule GaussLobattoSubdivided(int n, int subdivisions, int dimension)
        {
            QuadratureRule oneD = Rules.GaussLobatto.Rule(n);
            QuadratureRule subD = Subdivide(oneD, subdivisions);
            return Tensorize(subD, dimension);
        }

        public static QuadratureRule BruteForce(int n, int dimension)
        {
            QuadratureRule oneD = Rules.BruteForce.Rule(n);
            return Tensorize(oneD, dimension);
        }

        public static QuadratureRule Plot(int n, int dimension)
        {
            QuadratureRule oneD = Rules.Plot.Rule(n);
            return Tensorize(oneD, dimension);
        }

        static QuadratureRule Subdivide(QuadratureRule rule, int subdivisions)
        {
            QuadratureRule subRule = new QuadratureRule(rule.Count * Algebra.Pow(2, subdivisions));
            double h = 1 / Math.Pow(2, subdivisions);
            for (int i = 0; i < Math.Pow(2, subdivisions); ++i)
            {
                for (int j = 0; j < rule.Count; ++j)
                {
                    Tensor1 point = rule[j].Point * h + Tensor1.Vector(-1) + h * (2 * i + 1) * Tensor1.Vector(1);
                    double weight = rule[j].Weight * h;
                    QuadratureNode node = new QuadratureNode()
                    {
                        Point = point,
                        Weight = weight
                    };
                    subRule.Add(node);
                }
            }
            return subRule;
        }

        static QuadratureRule Tensorize(QuadratureRule oneDimensional, int dimension)
        {
            int count = Algebra.Pow(oneDimensional.Count, dimension);
            QuadratureRule rule = QuadratureRule.Allocate(count, dimension);
            foreach (QuadratureNode node in rule)
            {
                node.Weight = 1.0;
            }

            for (int d = 0; d < dimension; ++d)
            {
                int i = 0;
                foreach (QuadratureNode node in rule)
                {
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

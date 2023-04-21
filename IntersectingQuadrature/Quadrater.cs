using IntersectingQuadrature.Tensor;
using System.Collections.Generic;
using IntersectingQuadrature.Map;

namespace IntersectingQuadrature
{
    public class Quadrater {

        Finder hunter;

        public Quadrater() {
            hunter = new Finder();
        }

        public QuadratureRule FindRule(IScalarFunction alpha, Symbol sign, HyperRectangle domain, int n, int subdivisions = 0) {
            QuadratureRule rules = new QuadratureRule(10);
            List<IntegralMapping> setA = hunter.FindMappings(alpha, sign, domain);
            foreach (IntegralMapping A in setA) {
                QuadratureRule gauss = QuadratureRules.GaussSubdivided(n, subdivisions, A.Domain.BodyDimension);
                QuadratureRule rule = Map(A.Transformation, gauss);
                rules.AddRange(rule);
            }
            return rules;
        }

        public QuadratureRule FindRule(IScalarFunction alpha, Symbol signAlpha, IScalarFunction beta, Symbol signBeta, HyperRectangle domain, int n, int subdivisions = 0) {
            QuadratureRule rules = new QuadratureRule(20);
            List <IntegralMapping> setAB = hunter.FindMappings(alpha, signAlpha, beta, signBeta, domain);
            foreach (IntegralMapping T in setAB) {
                //QuadratureRule gauss = QuadratureRules.Plot(3, B.Domain.Dimension);
                
                QuadratureRule gauss = QuadratureRules.GaussSubdivided(n, subdivisions, T.Domain.BodyDimension);
                QuadratureRule Q = Map(T.Transformation, gauss);
                rules.AddRange(Q);
                
            }
            return rules;
        }

        static QuadratureRule Map(IIntegralTransformation map, QuadratureRule source) {
            QuadratureRule target = new QuadratureRule(source.Count);
            for (int i = 0; i < source.Count; ++i) {
                QuadratureNode targeNode = Map(map, source[i]);
                target.Add(targeNode);
            };
            return target;
        }

        static QuadratureNode Map(IIntegralTransformation map, QuadratureNode source) {
            QuadratureNode target = new QuadratureNode();
            (double J, Tensor1 x) = map.EvaluateAndDeterminant(source.Point);
            target.Point = x;
            target.Weight = source.Weight * J;
            return target;
        }
    }
}

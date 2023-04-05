using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Mapper;
using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature {
    public class AdaptiveQuadrater {

        MapFinder hunter;
        Adapter adapter;

        public AdaptiveQuadrater(double tau) {
            adapter = new Adapter(tau);
            hunter = new MapFinder();
        }


        public QuadratureRule FindRule(IScalarFunction alpha, Symbol sign, HyperRectangle domain, int n, int subdivisions = 0) {
            QuadratureRule rules = new QuadratureRule(10);

            List<Map> setA = hunter.FindMappings(alpha, sign, domain);
            foreach (Map A in setA) {
                QuadratureRule gauss = QuadratureRules.Gauss(n, A.Domain.Dimension);
                QuadratureRule rule = Map(A.Mapping, gauss);
                rules.AddRange(rule);
            }
            return rules;
        }

        public QuadratureRule FindRule(IScalarFunction alpha, Symbol signAlpha, IScalarFunction beta, Symbol signBeta, HyperRectangle domain, int n, int subdivisions = 0) {
            QuadratureRule rules = new QuadratureRule(20);
            List <Map> setA = hunter.FindMappings(alpha, signAlpha, domain);

            IScalarFunction f = new ConstantPolynomial(1);
            foreach (Map A in setA) {
                IScalarFunction betaA = new ScalarComposition(beta, A.Mapping);
                List<Map> setB = hunter.FindMappings(betaA, signBeta, A.Domain);
                foreach (Map B in setB) {
                    IIntegralMapping AB = new MappingComposition(A.Mapping, B.Mapping);
                    QuadratureRule gauss = QuadratureRules.GaussSubdivided(n, subdivisions, B.Domain.Dimension);
                    QuadratureRule Q = adapter.FindRule(f, AB, B.Domain, gauss);
                    rules.AddRange(Q);
                }
                
            }
            return rules;
        }

        static QuadratureRule Map(IIntegralMapping map, QuadratureRule source) {
            QuadratureRule target = new QuadratureRule(source.Count);
            for (int i = 0; i < source.Count; ++i) {
                QuadratureNode targeNode = Map(map, source[i]);
                target.Add(targeNode);
            };
            return target;
        }

        static QuadratureNode Map(IIntegralMapping map, QuadratureNode source) {
            QuadratureNode target = new QuadratureNode();
            (double J, Tensor1 x) = map.EvaluateAndDeterminant(source.Point);
            target.Point = x;
            target.Weight = source.Weight * J;
            return target;
        }
    }
}

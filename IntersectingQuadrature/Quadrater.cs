using IntersectingQuadrature.Tensor;
using System.Collections.Generic;
using IntersectingQuadrature.Map;
using IntersectingQuadrature.Rules;
using System;

namespace IntersectingQuadrature
{
    public class Quadrater {

        Finder hunter;

        public Quadrater() {
            hunter = new Finder();
        }

        public QuadratureRule FindRule(IScalarFunction alpha, 
                                       Symbol sign, 
                                       IHyperRectangle domain, 
                                       int n, 
                                       int subdivisions = 0) {
            CheckInput(alpha, sign, domain, n, subdivisions);

            QuadratureRule rules = new QuadratureRule(10);
            List<IntegralMapping> setA = hunter.FindMappings(alpha, sign, domain);
            foreach (IntegralMapping A in setA) {
                QuadratureRule gauss = QuadratureRules.GaussSubdivided(n, subdivisions, A.Domain.Dimension);
                QuadratureRule rule = Map(A.Transformation, gauss);
                rules.AddRange(rule);
            }
            return rules;
        }

        public QuadratureRule FindRule(IScalarFunction alpha, 
                                       Symbol signAlpha, 
                                       IScalarFunction beta, 
                                       Symbol signBeta, 
                                       IHyperRectangle domain, 
                                       int n, 
                                       int subdivisions = 0) {
            CheckInput(alpha, signAlpha, beta, signBeta, domain, n, subdivisions);

            QuadratureRule rules = new QuadratureRule(20);
            List <IntegralMapping> setAB = hunter.FindMappings(alpha, signAlpha, beta, signBeta, domain);
            foreach (IntegralMapping T in setAB) {
                //QuadratureRule gauss = QuadratureRules.Plot(3, B.Domain.Dimension);
                
                QuadratureRule gauss = QuadratureRules.GaussSubdivided(n, subdivisions, T.Domain.Dimension);
                QuadratureRule Q = Map(T.Transformation, gauss);
                rules.AddRange(Q);
                
            }
            return rules;
        }

        static void CheckInput(IScalarFunction alpha,
                               Symbol sign,
                               IHyperRectangle domain,
                               int n,
                               int subdivisions = 0) {
            CheckLevelSet(alpha, sign, domain);
            CheckDomain(domain);
            CheckRuleInput(n, subdivisions);
        }

        static void CheckLevelSet(IScalarFunction alpha,
                               Symbol signAlpha,
                               IHyperRectangle domain) {
            if (alpha.M != domain.Dimension) {
                throw new NotSupportedException("Spatial dimension of level set and domain do not align");
            }
            if (signAlpha == Symbol.None) {
                throw new NotSupportedException("Sign must be minus, plus, or zero");
            }
        }

        static void CheckRuleInput(int n, int subdivisions) {
            if (n < 1 || n > 32) {
                throw new NotSupportedException("Number of nodes not supported");
            }
            if (subdivisions < 0) {
                throw new NotSupportedException("Subdivisions must be >= 0");
            }
        }

        static void CheckDomain(IHyperRectangle domain) {
            if(domain.Dimension < 1 || domain.Dimension > 3) {
                throw new NotSupportedException("Dimensions must be 1,2 or 3");
            }
            if (domain.Diameters.M != domain.Dimension) {
                throw new NotSupportedException("Length of diameter array and space dimensions do not match");
            }
            if (domain.Center.M != domain.Dimension) {
                throw new NotSupportedException("Length of active dimensions array and space dimensions do not match");
            }
        }

        static void CheckInput(IScalarFunction alpha,
                               Symbol signAlpha,
                               IScalarFunction beta,
                               Symbol signBeta,
                               IHyperRectangle domain,
                               int n,
                               int subdivisions) {
            CheckLevelSet(alpha, signAlpha, domain);
            CheckLevelSet(beta, signBeta, domain);
            CheckDomain(domain);
            CheckRuleInput(n, subdivisions);
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

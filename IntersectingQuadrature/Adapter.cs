using System;
using IntersectingQuadrature.Map;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature
{
    internal class Adapter {

        double tau = 0.0001;

        public Adapter(double tau) {
            this.tau = tau;
        }

        public QuadratureRule FindRule(IScalarFunction g, IIntegralTransformation map, HyperRectangle domain, QuadratureRule rule) {
            QuadratureRule rule0 = Map(map, rule);
            double s0 = Math.Abs(Quadrature.Evaluate(g, rule0));
            QuadratureRule result = new QuadratureRule(rule.Count);
            if (Adapt(g, map, domain, s0, rule, result, 5)) {
                result.AddRange(rule0);
            };
            return result;
        }


        bool Adapt(IScalarFunction g, IIntegralTransformation map, HyperRectangle domain, double s0, QuadratureRule baseRule, QuadratureRule result, int subdivs) {
            if(subdivs < 0) {
                return true;
            }
            HyperRectangle[] subdomains = Split(domain);
            double s1 = 0;
            double[] evaluations = new double[subdomains.Length];
            QuadratureRule[] rules = new QuadratureRule[subdomains.Length];
            for(int i = 0; i < subdomains.Length; ++i) {
                HyperRectangle subdomain = subdomains[i];
                QuadratureRule subRule = Rule(map, baseRule, subdomain);
                double s = Math.Abs(Quadrature.Evaluate(g, subRule));
                evaluations[i] = s;
                rules[i] = subRule;
                s1 += s;
            }
            double deltaS = Math.Abs((s0 - s1) / s0);
            if (deltaS < tau) {
                return true;
            } else {
                --subdivs;
                for (int i = 0; i < subdomains.Length; ++i) {
                    HyperRectangle subdomain = subdomains[i];
                    QuadratureRule subRule = rules[i];
                    double s = evaluations[i];
                    if (Adapt(g, map, subdomain, s, baseRule, result, subdivs)) {
                        result.AddRange(subRule);
                    };
                }
                return false;
            }
        }

        static HyperRectangle[] Split(HyperRectangle domain) {
            int n = Algebra.Pow(2, domain.BodyDimension);
            HyperRectangle[] subdomains = new HyperRectangle[n];
            for(int i = 0; i < n; ++i) {
                HyperRectangle subdomain = new HyperRectangle(domain.BodyDimension);
                subdomain.BodyDimension = domain.BodyDimension;
                int k = i;
                for (int j = 0; j < domain.BodyDimension; ++j) {
                    subdomain.Diameters[j] = domain.Diameters[j] / 2;
                    subdomain.Center[j] = domain.Center[j] - domain.Diameters[j] / 4 + (k%2) * domain.Diameters[j] / 2;
                    k /= 2;
                }
                subdomains[i] = subdomain;
            }
            return subdomains;
        }

        static QuadratureRule Rule(IIntegralTransformation map, QuadratureRule source, HyperRectangle domain) {
            Tensor2 scales = Tensor2.Zeros(domain.BodyDimension);
            for(int i = 0; i < domain.BodyDimension; ++i) {
                scales[i, i] = domain.Diameters[i] / 2;
            }
            LinearMapping t = new LinearMapping(domain.Center, scales);
            QuadratureRule gaussT = Map(t, source);
            return Map(map, gaussT);
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

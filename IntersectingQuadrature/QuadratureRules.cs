using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    internal static class QuadratureRules {
        
        public static QuadratureRule Gauss(int n, int dimension) {
            QuadratureRule oneD = Gauss(n);
            return Tensorize(oneD, dimension);
        }

        public static QuadratureRule GaussSubdivided(int n, int subdivisions, int dimension) {
            QuadratureRule oneD = Gauss(n);
            QuadratureRule subD = Subdivide(oneD, subdivisions);
            return Tensorize(subD, dimension);
        }

        public static QuadratureRule GaussTschebychow(int n, int dimension) {
            QuadratureRule oneD = GaussTschebychow(n);
            return Tensorize(oneD, dimension);
        }

        public static QuadratureRule GaussLobatto(int n, int dimension) {
            QuadratureRule oneD = GaussLobatto(n);
            return Tensorize(oneD, dimension);
        }

        public static QuadratureRule GaussLobattoSubdivided(int n, int subdivisions, int dimension) {
            QuadratureRule oneD = GaussLobatto(n);
            QuadratureRule subD = Subdivide(oneD, subdivisions);
            return Tensorize(subD, dimension);
        }

        public static QuadratureRule BruteForce(int n, int dimension) {
            QuadratureRule oneD = BruteForce(n);
            return Tensorize(oneD, dimension);
        }

        public static QuadratureRule Plot(int n, int dimension) {
            QuadratureRule oneD = Plot(n);
            return Tensorize(oneD, dimension);
        }

        static QuadratureRule Subdivide(QuadratureRule rule, int subdivisions) {
            QuadratureRule subRule = new QuadratureRule(rule.Count * MathUtility.Pow(2, subdivisions));
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

        static QuadratureRule Tensorize(QuadratureRule oneDimensional, int dimension) {
            int count = MathUtility.Pow(oneDimensional.Count, dimension);
            QuadratureRule rule = QuadratureRule.Allocate(count, dimension);
            foreach (QuadratureNode node in rule) {
                node.Weight = 1.0;
            }

            for (int d = 0; d < dimension; ++d) {
                int i = 0;
                foreach (QuadratureNode node in rule) {
                    int j = (i / MathUtility.Pow(oneDimensional.Count, d)) % oneDimensional.Count;
                    node.Point[d] = oneDimensional[j].Point[0];
                    node.Weight *= oneDimensional[j].Weight;
                    ++i;
                }
            }
            return rule;
        }

        public static QuadratureRule Gauss(int n) {
            QuadratureRule gaussRule = new QuadratureRule(n);
            switch (n) {
                case 1:
                gaussRule.Add( new QuadratureNode() {
                    Point = Tensor1.Vector(0),
                    Weight= 2
                });
                break;
                case 2:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-0.5773502691896257 ),
                    Weight = 1
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector( 0.5773502691896257 ),
                    Weight = 1
                });
                break;
                case 3:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-0.7745966692414834 ),
                    Weight = 0.5555555555555556
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0),
                    Weight = 0.8888888888888888
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0.7745966692414834),
                    Weight = 0.5555555555555556
                });
                break;
                case 4:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-0.8611363115940526),
                    Weight = 0.3478548451374538
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-0.3399810435848563),
                    Weight = 0.6521451548625461
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0.3399810435848563),
                    Weight = 0.6521451548625461
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0.8611363115940526),
                    Weight = 0.3478548451374538
                });
                break;
                case 5:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-0.9061798459386640),
                    Weight = 0.2369268850561891
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-0.5384693101056831),
                    Weight = 0.4786286704993665
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0.0000000000000000),
                    Weight = 0.5688888888888889
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0.5384693101056831),
                    Weight = 0.4786286704993665
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0.9061798459386640),
                    Weight = 0.2369268850561891
                });
                break;
                default:
                throw new NotImplementedException();
            }
            return gaussRule;
        }

        public static QuadratureRule BruteForce(int n) {
            QuadratureRule rule = new QuadratureRule(n);
            double increment = 2.0 / (n - 1);
            for(int i = 0; i < n; ++i) {
                rule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1 + i * increment),
                    Weight = increment
                });
            }
            rule.First().Weight /= 2;
            rule.Last().Weight /= 2;
            return rule;
        }

        public static QuadratureRule Plot(int n) {
            QuadratureRule rule = new QuadratureRule(n);
            double increment = 1.98 / (n - 1);
            for (int i = 0; i < n; ++i) {
                rule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-0.99 + i * increment),
                    Weight = 1.0
                });
            }
            return rule;
        }

        public static QuadratureRule TanhSinh(int n) {
            QuadratureRule tanhRule = new QuadratureRule(n);
            throw new NotImplementedException();
            return tanhRule;
        }

        public static QuadratureRule GaussTschebychow(int n) {
            QuadratureRule gaussRule = new QuadratureRule(n);
            double pi = Math.PI / n;
            for (int i = 0; i < n; ++i) {
                Tensor1 x = Tensor1.Vector(Math.Cos((2 * i + 1) / (2.0 * n) * Math.PI));
                double w = Math.Sin((2 * i + 1) / (2.0 * n) * Math.PI); 
                gaussRule.Add(new QuadratureNode() {
                    Point = x,
                    Weight = pi *  w
                }) ;
            }
            return gaussRule;
        }

        public static QuadratureRule GaussLobatto(int n) {
            QuadratureRule gaussRule = new QuadratureRule(n);
            switch (n) {
                case 2:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1),
                    Weight = 1
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(1),
                    Weight = 1
                });
                break;
                case 3:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1),
                    Weight = 1.0 / 3.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0),
                    Weight = 4.0/3.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(1),
                    Weight = 1.0 / 3.0
                });
                break;
                case 4:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1),
                    Weight = 1.0 / 6.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-Math.Sqrt(1.0/5.0)),
                    Weight = 5.0 / 6.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(Math.Sqrt(1.0 / 5.0)),
                    Weight = 5.0 / 6.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(1),
                    Weight = 1.0 / 6.0
                });
                break;
                case 5:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1),
                    Weight = 1.0 / 10.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-Math.Sqrt(3.0 / 7.0 )),
                    Weight = 49.0 / 90.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0.0000000000000000),
                    Weight = 32.0 / 45.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(Math.Sqrt(3.0 / 7.0)),
                    Weight = 49.0 / 90.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(1),
                    Weight = 1.0 / 10.0
                });
                break;
                default:
                throw new NotImplementedException();
            }
            return gaussRule;
        }
    }
}

using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntersectingQuadrature.Rules {
    internal class BruteForce {
        public static QuadratureRule Rule(int n) {
            QuadratureRule rule = new QuadratureRule(n);
            double increment = 2.0 / (n - 1);
            for (int i = 0; i < n; ++i) {
                rule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1 + i * increment),
                    Weight = increment
                });
            }
            rule.First().Weight /= 2;
            rule.Last().Weight /= 2;
            return rule;
        }
    }
}

using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Rules {
    internal class Plot {
        public static QuadratureRule Rule(int n) {
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
    }
}

using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Rules {
    internal class GaussTschebychow {
        public static QuadratureRule Rule(int n) {
            QuadratureRule gaussRule = new QuadratureRule(n);
            double pi = Math.PI / n;
            for (int i = 0; i < n; ++i) {
                Tensor1 x = Tensor1.Vector(Math.Cos((2 * i + 1) / (2.0 * n) * Math.PI));
                double w = Math.Sin((2 * i + 1) / (2.0 * n) * Math.PI);
                gaussRule.Add(new QuadratureNode() {
                    Point = x,
                    Weight = pi * w
                });
            }
            return gaussRule;
        }
    }
}
